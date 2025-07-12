"use client"

import Link from "next/link"
import { Card, CardContent, CardHeader } from "@/components/ui/card"
import { Badge } from "@/components/ui/badge"
import { Button } from "@/components/ui/button"
import { ArrowUp, ArrowDown, MessageSquare, Check } from "lucide-react"
import type { Question } from "@/lib/types"
import { useAuth } from "@/lib/auth-context"
import { db } from "@/lib/database"

interface QuestionCardProps {
  question: Question
  onVote?: () => void
}

export function QuestionCard({ question, onVote }: QuestionCardProps) {
  const { user } = useAuth()

  const handleVote = async (type: "up" | "down") => {
    if (!user) return
    await db.vote(user.id, question.id, "question", type)
    onVote?.()
  }

  // Gracefully handle a missing author object to avoid runtime errors
  const authorName = question.author?.username ?? "Unknown"
  const createdDate = question.createdAt ? question.createdAt.toLocaleDateString() : new Date().toLocaleDateString()

  return (
    <Card className="mb-4">
      <CardHeader className="pb-3">
        <div className="flex items-start justify-between">
          <div className="flex-1">
            <Link
              href={`/questions/${question.id}`}
              className="text-lg font-semibold text-blue-600 hover:text-blue-800"
            >
              {question.title}
            </Link>
            <div className="flex items-center gap-2 mt-2 text-sm text-gray-500">
              <span>by {authorName}</span>
              <span>•</span>
              <span>{createdDate}</span>
              {question.acceptedAnswerId && (
                <>
                  <span>•</span>
                  <div className="flex items-center gap-1 text-green-600">
                    <Check className="w-4 h-4" />
                    <span>Answered</span>
                  </div>
                </>
              )}
            </div>
          </div>
          <div className="flex items-center gap-2">
            {user && (
              <div className="flex flex-col items-center">
                <Button variant="ghost" size="sm" onClick={() => handleVote("up")}>
                  <ArrowUp className="w-4 h-4" />
                </Button>
                <span className="text-sm font-medium">{question.votes}</span>
                <Button variant="ghost" size="sm" onClick={() => handleVote("down")}>
                  <ArrowDown className="w-4 h-4" />
                </Button>
              </div>
            )}
          </div>
        </div>
      </CardHeader>
      <CardContent>
        <div className="text-gray-700 mb-3 line-clamp-3" dangerouslySetInnerHTML={{ __html: question.description }} />
        <div className="flex items-center justify-between">
          <div className="flex flex-wrap gap-2">
            {question.tags.map((tag) => (
              <Badge key={tag} variant="secondary">
                {tag}
              </Badge>
            ))}
          </div>
          <div className="flex items-center gap-1 text-sm text-gray-500">
            <MessageSquare className="w-4 h-4" />
            <span>View answers</span>
          </div>
        </div>
      </CardContent>
    </Card>
  )
}
