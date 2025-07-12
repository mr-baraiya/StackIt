export interface User {
  id: string
  username: string
  email: string
  role: "guest" | "user" | "admin"
  createdAt: Date
}

export interface Question {
  id: string
  title: string
  description: string
  tags: string[]
  authorId: string
  author: User
  createdAt: Date
  updatedAt: Date
  votes: number
  acceptedAnswerId?: string
}

export interface Answer {
  id: string
  content: string
  questionId: string
  authorId: string
  author: User
  createdAt: Date
  updatedAt: Date
  votes: number
  isAccepted: boolean
}

export interface Vote {
  id: string
  userId: string
  targetId: string
  targetType: "question" | "answer"
  type: "up" | "down"
}

export interface Notification {
  id: string
  userId: string
  type: "answer" | "comment" | "mention"
  message: string
  questionId?: string
  answerId?: string
  isRead: boolean
  createdAt: Date
}
