import { QuizQuestion } from "./quizQuestion";

export interface Quiz {
  difficulty: string,
  questions: QuizQuestion[],
  percentageScore: number,
  userId: number
}