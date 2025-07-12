"use client"

import type React from "react"

import { useState, useEffect } from "react"
import { useParams } from "next/navigation"
import { Header } from "@/components/header"
import { RichTextEditor } from "@/components/rich-text-editor"
import { Button } from "@/components/ui/button"
import { Badge } from "@/components/ui/badge"
import { Card, CardContent, CardHeader } from "@/components/ui/card"
import { ArrowUp, ArrowDown, Check } from "lucide-react"
import type { Question, Answer } from "@/lib/types"
import { useAuth } from "@/lib/auth-context"
import { db } from "@/lib/database"

export default function QuestionDetailPage() {
  const params = useParams()
  const { user } = useAuth()
  const [question, setQuestion] = useState<Question | null>(null)
  const [answers, setAnswers] = useState<Answer[]>([])
  const [newAnswer, setNewAnswer] = useState("")
  const [loading, setLoading] = useState(true)
  const [submitting, setSubmitting] = useState(false)

  useEffect(() => {
    if (params.id) {
      loadQuestionAndAnswers()
    }
  }, [params.id])

  const loadQuestionAndAnswers = async () => {
    try {
      const questionData = await db.getQuestionById(params.id as string)
      const answersData = await db.getAnswersByQuestionId(params.id as string)

      setQuestion(questionData)
      setAnswers(answersData)
    } catch (error) {
      console.error("Error loading question:", error)
    } finally {
      setLoading(false)
    }
  }

  const handleVoteQuestion = async (type: "up" | "down") => {
    if (!user || !question) return
    await db.vote(user.id, question.id, "question", type)
    loadQuestionAndAnswers()
  }

  const handleVoteAnswer = async (answerId: string, type: "up" | "down") => {
    if (!user) return
    await db.vote(user.id, answerId, "answer", type)
    loadQuestionAndAnswers()
  }

  const handleAcceptAnswer = async (answerId: string) => {
    if (!user || !question || user.id !== question.authorId) return
    await db.acceptAnswer(answerId, question.id)
    loadQuestionAndAnswers()
  }

  const handleSubmitAnswer = async (e: React.FormEvent) => {
    e.preventDefault()
    if (!user || !question || !newAnswer.trim()) return

    setSubmitting(true)
    try {
      await db.createAnswer({
        content: newAnswer.trim(),
        questionId: question.id,
        authorId: user.id,
      })
      setNewAnswer("")
      loadQuestionAndAnswers()
    } catch (error) {
      console.error("Error submitting answer:", error)
      alert("Error submitting answer. Please try again.")
    } finally {
      setSubmitting(false)
    }
  }

  if (loading) {
    return (
      <div className="min-h-screen bg-gray-50">
        <Header />
        <main className="container mx-auto px-4 py-8">
          <p className="text-center text-gray-500">Loading question...</p>
        </main>
      </div>
    )
  }

  if (!question) {
    return (
      <div className="min-h-screen bg-gray-50">
        <Header />
        <main className="container mx-auto px-4 py-8">
          <p className="text-center text-gray-500">Question not found.</p>
        </main>
      </div>
    )
  }

  return (
    <div className="min-h-screen bg-gray-50">
      <Header />

      <main className="container mx-auto px-4 py-8">
        <div className="max-w-4xl mx-auto">
          {/* Question */}
          <Card className="mb-8">
            <CardHeader>
              <div className="flex items-start gap-4">
                {user && (
                  <div className="flex flex-col items-center">
                    <Button variant="ghost" size="sm" onClick={() => handleVoteQuestion("up")}>
                      <ArrowUp className="w-5 h-5" />
                    </Button>
                    <span className="text-lg font-medium">{question.votes}</span>
                    <Button variant="ghost" size="sm" onClick={() => handleVoteQuestion("down")}>
                      <ArrowDown className="w-5 h-5" />
                    </Button>
                  </div>
                )}
                <div className="flex-1">
                  <h1 className="text-2xl font-bold text-gray-900 mb-2">{question.title}</h1>
                  <div className="flex items-center gap-2 text-sm text-gray-500 mb-4">
                    <span>Asked by {question.author.username}</span>
                    <span>•</span>
                    <span>{question.createdAt.toLocaleDateString()}</span>
                  </div>
                  <div className="prose max-w-none mb-4" dangerouslySetInnerHTML={{ __html: question.description }} />
                  <div className="flex flex-wrap gap-2">
                    {question.tags.map((tag) => (
                      <Badge key={tag} variant="secondary">
                        {tag}
                      </Badge>
                    ))}
                  </div>
                </div>
              </div>
            </CardHeader>
          </Card>

          {/* Answers */}
          <div className="mb-8">
            <h2 className="text-xl font-semibold mb-4">
              {answers.length} {answers.length === 1 ? "Answer" : "Answers"}
            </h2>

            {answers.map((answer) => (
              <Card key={answer.id} className={`mb-4 ${answer.isAccepted ? "border-green-500 bg-green-50" : ""}`}>
                <CardContent className="pt-6">
                  <div className="flex items-start gap-4">
                    {user && (
                      <div className="flex flex-col items-center">
                        <Button variant="ghost" size="sm" onClick={() => handleVoteAnswer(answer.id, "up")}>
                          <ArrowUp className="w-5 h-5" />
                        </Button>
                        <span className="text-lg font-medium">{answer.votes}</span>
                        <Button variant="ghost" size="sm" onClick={() => handleVoteAnswer(answer.id, "down")}>
                          <ArrowDown className="w-5 h-5" />
                        </Button>
                        {user.id === question.authorId && !answer.isAccepted && (
                          <Button
                            variant="ghost"
                            size="sm"
                            onClick={() => handleAcceptAnswer(answer.id)}
                            className="mt-2 text-green-600 hover:text-green-700"
                          >
                            <Check className="w-5 h-5" />
                          </Button>
                        )}
                      </div>
                    )}
                    <div className="flex-1">
                      {answer.isAccepted && (
                        <div className="flex items-center gap-2 mb-2 text-green-600">
                          <Check className="w-4 h-4" />
                          <span className="text-sm font-medium">Accepted Answer</span>
                        </div>
                      )}
                      <div className="prose max-w-none mb-4" dangerouslySetInnerHTML={{ __html: answer.content }} />
                      <div className="text-sm text-gray-500">
                        Answered by {answer.author.username} on {answer.createdAt.toLocaleDateString()}
                      </div>
                    </div>
                  </div>
                </CardContent>
              </Card>
            ))}
          </div>

          {/* Answer Form */}
          {user ? (
            <Card>
              <CardHeader>
                <h3 className="text-lg font-semibold">Your Answer</h3>
              </CardHeader>
              <CardContent>
                <form onSubmit={handleSubmitAnswer}>
                  <RichTextEditor value={newAnswer} onChange={setNewAnswer} placeholder="Write your answer here..." />
                  <div className="mt-4">
                    <Button type="submit" disabled={submitting || !newAnswer.trim()}>
                      {submitting ? "Submitting..." : "Post Answer"}
                    </Button>
                  </div>
                </form>
              </CardContent>
            </Card>
          ) : (
            <Card>
              <CardContent className="pt-6 text-center">
                <p className="text-gray-500 mb-4">You need to be logged in to post an answer.</p>
                <Button onClick={() => (window.location.href = "/login")}>Login</Button>
              </CardContent>
            </Card>
          )}
        </div>
      </main>
    </div>
  )
}
