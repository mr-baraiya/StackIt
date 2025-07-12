"use client"

import { useState, useEffect } from "react"
import { Header } from "@/components/header"
import { QuestionCard } from "@/components/question-card"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Search, Plus } from "lucide-react"
import type { Question } from "@/lib/types"
import { db } from "@/lib/database"
import Link from "next/link"

export default function HomePage() {
  const [questions, setQuestions] = useState<Question[]>([])
  const [searchTerm, setSearchTerm] = useState("")
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    loadQuestions()
  }, [])

  const loadQuestions = async () => {
    try {
      const allQuestions = await db.getQuestions()
      setQuestions(allQuestions)
    } catch (error) {
      console.error("Error loading questions:", error)
    } finally {
      setLoading(false)
    }
  }

  const filteredQuestions = questions.filter(
    (question) =>
      question.title.toLowerCase().includes(searchTerm.toLowerCase()) ||
      question.tags.some((tag) => tag.toLowerCase().includes(searchTerm.toLowerCase())),
  )

  return (
    <div className="min-h-screen bg-gray-50">
      <Header />

      <main className="container mx-auto px-4 py-8">
        <div className="flex items-center justify-between mb-8">
          <h1 className="text-3xl font-bold text-gray-900">All Questions</h1>
          <Link href="/ask">
            <Button>
              <Plus className="w-4 h-4 mr-2" />
              Ask Question
            </Button>
          </Link>
        </div>

        <div className="mb-6">
          <div className="relative">
            <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400 w-5 h-5" />
            <Input
              placeholder="Search questions or tags..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="pl-10"
            />
          </div>
        </div>

        {loading ? (
          <div className="text-center py-8">
            <p className="text-gray-500">Loading questions...</p>
          </div>
        ) : filteredQuestions.length === 0 ? (
          <div className="text-center py-8">
            <p className="text-gray-500 mb-4">
              {searchTerm ? "No questions found matching your search." : "No questions yet."}
            </p>
            <Link href="/ask">
              <Button>Ask the first question</Button>
            </Link>
          </div>
        ) : (
          <div>
            {filteredQuestions.map((question) => (
              <QuestionCard key={question.id} question={question} onVote={loadQuestions} />
            ))}
          </div>
        )}
      </main>
    </div>
  )
}
