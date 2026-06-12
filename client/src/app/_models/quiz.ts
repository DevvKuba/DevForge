import { QuizQuestion } from "./quizQuestion";

export interface Quiz {
  id: number | null,
  difficulty: string,
  questions: QuizQuestion[],
  percentageScore: number,
  isComplete: boolean,
  userId: number
}