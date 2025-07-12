"use client"

import { useRef } from "react"
import { Button } from "@/components/ui/button"
import {
  Bold,
  Italic,
  Strikethrough,
  List,
  ListOrdered,
  Link,
  ImageIcon,
  AlignLeft,
  AlignCenter,
  AlignRight,
  Smile,
} from "lucide-react"

interface RichTextEditorProps {
  value: string
  onChange: (value: string) => void
  placeholder?: string
}

export function RichTextEditor({ value, onChange, placeholder }: RichTextEditorProps) {
  const editorRef = useRef<HTMLDivElement>(null)

  const executeCommand = (command: string, value?: string) => {
    document.execCommand(command, false, value)
    if (editorRef.current) {
      onChange(editorRef.current.innerHTML)
    }
  }

  const insertEmoji = () => {
    const emoji = prompt("Enter emoji:")
    if (emoji) {
      executeCommand("insertText", emoji)
    }
  }

  const insertLink = () => {
    const url = prompt("Enter URL:")
    if (url) {
      executeCommand("createLink", url)
    }
  }

  const insertImage = () => {
    const url = prompt("Enter image URL:")
    if (url) {
      executeCommand("insertImage", url)
    }
  }

  return (
    <div className="border rounded-lg">
      <div className="flex flex-wrap gap-1 p-2 border-b bg-gray-50">
        <Button type="button" variant="ghost" size="sm" onClick={() => executeCommand("bold")}>
          <Bold className="w-4 h-4" />
        </Button>
        <Button type="button" variant="ghost" size="sm" onClick={() => executeCommand("italic")}>
          <Italic className="w-4 h-4" />
        </Button>
        <Button type="button" variant="ghost" size="sm" onClick={() => executeCommand("strikeThrough")}>
          <Strikethrough className="w-4 h-4" />
        </Button>
        <div className="w-px h-6 bg-gray-300 mx-1" />
        <Button type="button" variant="ghost" size="sm" onClick={() => executeCommand("insertOrderedList")}>
          <ListOrdered className="w-4 h-4" />
        </Button>
        <Button type="button" variant="ghost" size="sm" onClick={() => executeCommand("insertUnorderedList")}>
          <List className="w-4 h-4" />
        </Button>
        <div className="w-px h-6 bg-gray-300 mx-1" />
        <Button type="button" variant="ghost" size="sm" onClick={insertEmoji}>
          <Smile className="w-4 h-4" />
        </Button>
        <Button type="button" variant="ghost" size="sm" onClick={insertLink}>
          <Link className="w-4 h-4" />
        </Button>
        <Button type="button" variant="ghost" size="sm" onClick={insertImage}>
          <ImageIcon className="w-4 h-4" />
        </Button>
        <div className="w-px h-6 bg-gray-300 mx-1" />
        <Button type="button" variant="ghost" size="sm" onClick={() => executeCommand("justifyLeft")}>
          <AlignLeft className="w-4 h-4" />
        </Button>
        <Button type="button" variant="ghost" size="sm" onClick={() => executeCommand("justifyCenter")}>
          <AlignCenter className="w-4 h-4" />
        </Button>
        <Button type="button" variant="ghost" size="sm" onClick={() => executeCommand("justifyRight")}>
          <AlignRight className="w-4 h-4" />
        </Button>
      </div>
      <div
        ref={editorRef}
        contentEditable
        className="min-h-[200px] p-4 focus:outline-none"
        dangerouslySetInnerHTML={{ __html: value }}
        onInput={(e) => onChange(e.currentTarget.innerHTML)}
        data-placeholder={placeholder}
        style={{
          minHeight: "200px",
        }}
      />
    </div>
  )
}
