using System;

// Adaptive Math Quiz
// Console application demonstrating delegates, events, object-oriented programming,
// and dynamic difficulty adjustment based on user performance.

namespace adaptiveMathQuiz
{
    delegate void MathQuizDele(bool increaseDifficulty);//delegate for the event //with a bool parameter (event)
    class adaptiveMathQuiz
    {
        public static void Main (string[] arg)
        {
            bool keepGoing = true;
            int totalCorrect = 0;
            int totalIncorrect = 0;

            int consecutiveCorrect = 0;
            int consecutiveIncorrect = 0;

            // Initialize difficulty level
            DifficultyTracker.level = 2;
            DifficultyManager manager = new DifficultyManager();
            DifficultyTracker tracker = new DifficultyTracker();
            manager.MathQuizEvent += tracker.AdjustDifficulty;

            Random random = new Random();

            // Repeat until the user wants to stop:
            while(keepGoing)
            {
                // i. Tell the user number of correct and incorrect answers so far
                Console.WriteLine("\nCorrect Answers: {0}, Incorrect Answers: {1}",
                totalCorrect, totalIncorrect);
                // ii. Tell the user the current difficulty level, default difficulty is level 2
                Console.WriteLine("Current Difficulty Level: {0}",DifficultyTracker.level);

                // 1. There are 4 difficulty levels:
                // a. On level 1, only addition problems are shown to the user
                // b. On level 2, addition and subtraction problems are shown
                // c. On level 3, addition, subtraction, and multiplication problems are shown
                // d. On level 4, addition, subtraction, and division problems are shown
                int num1 = random.Next(1, 11);
                int num2 = random.Next(1, 11);
                double correctAnswer = 0;
                string operation = "";

                switch (DifficultyTracker.level)
                {
                    case 1: // Only addition
                        operation = "+";
                        correctAnswer = num1 + num2;
                        break;
                    case 2: // Addition and subtraction
                        if (random.Next(2) == 0)
                        {
                            operation = "+";
                            correctAnswer = num1 + num2;
                        }
                        else
                        {
                            operation = "-";
                            correctAnswer = num1 - num2;
                        }
                        break;
                    case 3: // Addition, subtraction, and multiplication
                        int opChoice = random.Next(3);
                        if (opChoice == 0)
                        {
                            operation = "+";
                            correctAnswer = num1 + num2;
                        }
                        else if (opChoice == 1)
                        {
                            operation = "-";
                            correctAnswer = num1 - num2;
                        }
                        else
                        {
                            operation = "*";
                            correctAnswer = num1 * num2;
                        }
                        break;
                    case 4: // Addition, subtraction, and division
                        int opChoice4 = random.Next(3);
                        if (opChoice4 == 0)
                        {
                            operation = "+";
                            correctAnswer = num1 + num2;
                        }
                        else if (opChoice4 == 1)
                        {
                            operation = "-";
                            correctAnswer = num1 - num2;
                        }
                        else
                        {
                            operation = "/";
                            correctAnswer = Math.Round((double)num1 / num2, 2);
                        }
                        break;
                    default:
                        Console.WriteLine("Unexpected difficulty level. Defaulting to addition.");
                        operation = "+";
                        correctAnswer = num1 + num2;
                        break;
                }
                // iii. Display a random math problem to the user with 2 integer values between 1 – 10 based on
                // difficulty level
                // iv. Round the answer to the 2nd decimal point
                Console.Write("Solve: {0} {1} {2} = ", num1, operation, num2);
                if (double.TryParse(Console.ReadLine(), out double userAnswer) && userAnswer == correctAnswer)
                {
                    Console.WriteLine("Correct!");
                    totalCorrect++;
                    consecutiveCorrect++;

                    consecutiveIncorrect = 0;
                    // reset incorrect count
                }
                else
                {
                    Console.WriteLine("Incorrect :( The correct answer was {0}",correctAnswer);
                    totalIncorrect++;
                    consecutiveIncorrect++;

                    consecutiveCorrect = 0;
                    // reset correct count
                }
                // v. Increase the difficulty if the user gets 2 questions correct in a row
                // vi. Decrease the difficulty if the user gets 2 questions incorrect in a row
                if (consecutiveCorrect == 2)
                {
                    manager.OnChangeDifficulty(true);
                    consecutiveCorrect = 0;
                }
                else if (consecutiveIncorrect == 2)
                {
                    manager.OnChangeDifficulty(false);
                    consecutiveIncorrect = 0;
                }

                Console.Write("Do you want to continue? (yes/no): ");
                keepGoing = Console.ReadLine()?.ToLower() == "yes";
            }
            
            int totalQuestions = totalCorrect + totalIncorrect;

            double accuracy = 0;

            if (totalQuestions > 0)
            {
                accuracy = (double)totalCorrect / totalQuestions * 100;
            }

            Console.WriteLine("\n===== Final Score =====");
            Console.WriteLine("Correct Answers: " + totalCorrect);
            Console.WriteLine("Incorrect Answers: " + totalIncorrect);
            Console.WriteLine("Questions Answered: " + totalQuestions);
            Console.WriteLine("Accuracy: {0:F1}%", accuracy);
            Console.WriteLine("Final Difficulty Level: " + DifficultyTracker.level);
            Console.WriteLine("Thanks for playing!");

        }//main
    }//class adaptiveMathQuiz
    class DifficultyManager//Create a publisher class:
    {
        // a. Create an event called ChangeDifficulty with a bool parameter
        public event MathQuizDele MathQuizEvent;

        // b. Create a method called OnChangeDifficulty to invoke the event
        public void OnChangeDifficulty(bool increaseDifficulty)//instance method
        {
            MathQuizEvent?.Invoke(increaseDifficulty); // invoke event with difficulty change
        }
    }//DifficultyManager Class
    class DifficultyTracker//Create a subscriber class:
    {
        // a. Create a way to track the difficulty level. Hint: a static variable would be useful here
        public static int level = 2;//default

        // b. Create a method called AdjustDifficulty  bool parameter. The method will either
        // increase the difficulty if the bool is true or decrease it is if it false
        
        public void AdjustDifficulty(bool increaseDifficulty)
        {
            if (increaseDifficulty && level < 4)//also checks level to keep in bounds
            {
                level++;
                Console.WriteLine("Difficulty increased to level " + level);
            }
            else if (!increaseDifficulty && level > 1)
            {
                level--;
                Console.WriteLine("Difficulty decreased to level " + level);
            }
        }

    }//DifficultyTracker class
}//namespace adaptiveMathQuiz
