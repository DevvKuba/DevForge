import { QuizQuestion } from "./quizQuestion";

export interface Quiz {
  id: number | null,
  difficulty: string,
  questions: QuizQuestion[],
  percentageScore: number,
  userId: number
}