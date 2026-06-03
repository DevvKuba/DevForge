export interface QuizQuestion {
  type: string,
  difficulty: string,
  category: string,
  question: string,
  correctAnswer: string,
  incorrectAnswers: string[]
}