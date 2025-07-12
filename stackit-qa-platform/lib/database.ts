import type { User, Question, Answer, Vote, Notification } from "./types"

// Mock database - in a real app, this would be a proper database
class MockDatabase {
  private users: User[] = [
    {
      id: "1",
      username: "john_doe",
      email: "john@example.com",
      role: "user",
      createdAt: new Date("2024-01-01"),
    },
    {
      id: "2",
      username: "jane_smith",
      email: "jane@example.com",
      role: "user",
      createdAt: new Date("2024-01-02"),
    },
  ]

  private questions: Question[] = [
    {
      id: "1",
      title: "How to implement authentication in Next.js?",
      description: "I need help setting up authentication in my Next.js application. What are the best practices?",
      tags: ["nextjs", "authentication", "react"],
      authorId: "1",
      author: this.users[0],
      createdAt: new Date("2024-01-10"),
      updatedAt: new Date("2024-01-10"),
      votes: 5,
    },
  ]

  private answers: Answer[] = [
    {
      id: "1",
      content: "You can use NextAuth.js for authentication. It provides multiple providers and is easy to set up.",
      questionId: "1",
      authorId: "2",
      author: this.users[1],
      createdAt: new Date("2024-01-11"),
      updatedAt: new Date("2024-01-11"),
      votes: 3,
      isAccepted: false,
    },
  ]

  private votes: Vote[] = []
  private notifications: Notification[] = []

  // Users
  async getUsers(): Promise<User[]> {
    return this.users
  }

  async getUserById(id: string): Promise<User | null> {
    return this.users.find((user) => user.id === id) || null
  }

  async createUser(userData: Omit<User, "id" | "createdAt">): Promise<User> {
    const user: User = {
      ...userData,
      id: Date.now().toString(),
      createdAt: new Date(),
    }
    this.users.push(user)
    return user
  }

  // Questions
  async getQuestions(): Promise<Question[]> {
    return this.questions.map((q) => ({
      ...q,
      author: this.users.find((u) => u.id === q.authorId)!,
    }))
  }

  async getQuestionById(id: string): Promise<Question | null> {
    const question = this.questions.find((q) => q.id === id)
    if (!question) return null
    return {
      ...question,
      author: this.users.find((u) => u.id === question.authorId)!,
    }
  }

  async createQuestion(
    questionData: Omit<Question, "id" | "createdAt" | "updatedAt" | "votes" | "author">,
  ): Promise<Question> {
    const question: Question = {
      ...questionData,
      id: Date.now().toString(),
      createdAt: new Date(),
      updatedAt: new Date(),
      votes: 0,
      author: this.users.find((u) => u.id === questionData.authorId)!,
    }
    this.questions.push(question)
    return question
  }

  // Answers
  async getAnswersByQuestionId(questionId: string): Promise<Answer[]> {
    return this.answers
      .filter((a) => a.questionId === questionId)
      .map((a) => ({
        ...a,
        author: this.users.find((u) => u.id === a.authorId)!,
      }))
  }

  async createAnswer(
    answerData: Omit<Answer, "id" | "createdAt" | "updatedAt" | "votes" | "isAccepted" | "author">,
  ): Promise<Answer> {
    const answer: Answer = {
      ...answerData,
      id: Date.now().toString(),
      createdAt: new Date(),
      updatedAt: new Date(),
      votes: 0,
      isAccepted: false,
      author: this.users.find((u) => u.id === answerData.authorId)!,
    }
    this.answers.push(answer)

    // Create notification for question author
    const question = await this.getQuestionById(answerData.questionId)
    if (question && question.authorId !== answerData.authorId) {
      await this.createNotification({
        userId: question.authorId,
        type: "answer",
        message: `${answer.author.username} answered your question: ${question.title}`,
        questionId: answerData.questionId,
        answerId: answer.id,
        isRead: false,
      })
    }

    return answer
  }

  async acceptAnswer(answerId: string, questionId: string): Promise<void> {
    const answerIndex = this.answers.findIndex((a) => a.id === answerId)
    if (answerIndex !== -1) {
      // Reset all answers for this question
      this.answers.forEach((a) => {
        if (a.questionId === questionId) {
          a.isAccepted = false
        }
      })
      // Accept the selected answer
      this.answers[answerIndex].isAccepted = true

      // Update question
      const questionIndex = this.questions.findIndex((q) => q.id === questionId)
      if (questionIndex !== -1) {
        this.questions[questionIndex].acceptedAnswerId = answerId
      }
    }
  }

  // Votes
  async vote(
    userId: string,
    targetId: string,
    targetType: "question" | "answer",
    voteType: "up" | "down",
  ): Promise<void> {
    // Remove existing vote
    this.votes = this.votes.filter((v) => !(v.userId === userId && v.targetId === targetId))

    // Add new vote
    this.votes.push({
      id: Date.now().toString(),
      userId,
      targetId,
      targetType,
      type: voteType,
    })

    // Update vote count
    const upVotes = this.votes.filter((v) => v.targetId === targetId && v.type === "up").length
    const downVotes = this.votes.filter((v) => v.targetId === targetId && v.type === "down").length
    const totalVotes = upVotes - downVotes

    if (targetType === "question") {
      const questionIndex = this.questions.findIndex((q) => q.id === targetId)
      if (questionIndex !== -1) {
        this.questions[questionIndex].votes = totalVotes
      }
    } else {
      const answerIndex = this.answers.findIndex((a) => a.id === targetId)
      if (answerIndex !== -1) {
        this.answers[answerIndex].votes = totalVotes
      }
    }
  }

  // Notifications
  async getNotificationsByUserId(userId: string): Promise<Notification[]> {
    return this.notifications
      .filter((n) => n.userId === userId)
      .sort((a, b) => b.createdAt.getTime() - a.createdAt.getTime())
  }

  async createNotification(notificationData: Omit<Notification, "id" | "createdAt">): Promise<Notification> {
    const notification: Notification = {
      ...notificationData,
      id: Date.now().toString(),
      createdAt: new Date(),
    }
    this.notifications.push(notification)
    return notification
  }

  async markNotificationAsRead(notificationId: string): Promise<void> {
    const notificationIndex = this.notifications.findIndex((n) => n.id === notificationId)
    if (notificationIndex !== -1) {
      this.notifications[notificationIndex].isRead = true
    }
  }

  async getUnreadNotificationCount(userId: string): Promise<number> {
    return this.notifications.filter((n) => n.userId === userId && !n.isRead).length
  }
}

export const db = new MockDatabase()
