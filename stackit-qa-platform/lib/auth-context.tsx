"use client"

import { createContext, useContext, useState, useEffect, type ReactNode } from "react"
import type { User } from "./types"

interface AuthContextType {
  user: User | null
  login: (email: string, password: string) => Promise<boolean>
  logout: () => void
  register: (username: string, email: string, password: string) => Promise<boolean>
}

const AuthContext = createContext<AuthContextType | undefined>(undefined)

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<User | null>(null)

  useEffect(() => {
    // Check for stored user session
    const storedUser = localStorage.getItem("stackit-user")
    if (storedUser) {
      setUser(JSON.parse(storedUser))
    }
  }, [])

  const login = async (email: string, password: string): Promise<boolean> => {
    // Mock login - in real app, this would call an API
    const mockUser: User = {
      id: "1",
      username: "john_doe",
      email,
      role: "user",
      createdAt: new Date(),
    }
    setUser(mockUser)
    localStorage.setItem("stackit-user", JSON.stringify(mockUser))
    return true
  }

  const logout = () => {
    setUser(null)
    localStorage.removeItem("stackit-user")
  }

  const register = async (username: string, email: string, password: string): Promise<boolean> => {
    // Mock registration
    const mockUser: User = {
      id: Date.now().toString(),
      username,
      email,
      role: "user",
      createdAt: new Date(),
    }
    setUser(mockUser)
    localStorage.setItem("stackit-user", JSON.stringify(mockUser))
    return true
  }

  return <AuthContext.Provider value={{ user, login, logout, register }}>{children}</AuthContext.Provider>
}

export function useAuth() {
  const context = useContext(AuthContext)
  if (context === undefined) {
    throw new Error("useAuth must be used within an AuthProvider")
  }
  return context
}
