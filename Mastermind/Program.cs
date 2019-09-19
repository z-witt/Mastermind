using System;
using System.Linq;

namespace Mastermind
{
    class Program
    {
        const int NUM_OF_GUESSES = 10;
        const int LENGTH_OF_ANSWER = 4;
        const int MIN_DIGIT = 1;
        const int MAX_DIGIT = 6;
        const char CORRECT_POSITION_CHAR = '+';
        const char INCORRECT_POSITION_CHAR = '-';

        static void Main(string[] args)
        {
            bool hasWon = false;
            int turnsRemaining = NUM_OF_GUESSES;
            string answer = GenerateAnswer();

            Console.WriteLine("*****MASTERMIND*****");

            while(!hasWon && turnsRemaining > 0)
            {
                hasWon = TakeTurn(answer, turnsRemaining);
                turnsRemaining--;
            }

            if (hasWon)
                Console.WriteLine("YOU WON!");
            else
                Console.WriteLine("YOU LOST!");
        }

        /// <summary>
        /// Generates a random number from 1111 to 6666, with each digit ranging from 1 - 6.
        /// </summary>
        /// <returns>A string containing a number from 1111 to 6666, with each digit ranging from 1 - 6.</returns>
        private static string GenerateAnswer()
        {
            string answer = "";
            Random random = new Random();

            for (int i = 0; i < LENGTH_OF_ANSWER; i++)
            {
                answer = answer.Insert(answer.Length, random.Next(MIN_DIGIT, (MAX_DIGIT + 1)).ToString());
            }

            return answer;
        }

        /// <summary>
        /// Prompts user for input and validates that input. Displays result of correct digits.
        /// </summary>
        /// <param name="answer">The randomly generated answer.</param>
        /// <param name="turnsRemaining">The number of turns remaining.</param>
        /// <returns>A bool indicating if the user has won the game.</returns>
        private static bool TakeTurn(string answer, int turnsRemaining)
        {
            string guess = "";

            Console.WriteLine("Turns Remaining: " + turnsRemaining.ToString() + "\n");
            Console.WriteLine("Input your guess and press enter:");

            guess = Console.ReadLine();

            if(IsValidNumber(guess))
            {
                if(guess == answer)
                {
                    return true;
                }

                Console.WriteLine(GenerateResult(guess, answer));
            }
            else
            {
                Console.WriteLine("\nValid answer is between 1111 and 6666. Each digit ranges from 1 - 6.");
                TakeTurn(answer, turnsRemaining);
            }

            return false;
        }

        /// <summary>
        /// Validates the user input. A valid number is a number from 1111 to 6666, with each digit ranging from 1 - 6.
        /// </summary>
        /// <param name="guess">The input provided by the user.</param>
        /// <returns>A bool indicating if the input is valid or not.</returns>
        private static bool IsValidNumber(string guess)
        {
            bool valid = false;

            if (guess.Length == LENGTH_OF_ANSWER && guess.All(char.IsDigit))
            {
                valid = true;

                foreach (char digit in guess)
                {
                    int num = int.Parse(digit.ToString());

                    if (num < MIN_DIGIT || num > MAX_DIGIT)
                    {
                        valid = false;
                        break;
                    }
                }
            }

            return valid;
        }

        /// <summary>
        /// Generates the result of correct digits based on user's guess.
        /// </summary>
        /// <param name="guess">The input provided by the user.</param>
        /// <param name="answer">The randomly generated answer.</param>
        /// <returns>A string with the result to display to the user</returns>
        private static string GenerateResult(string guess, string answer)
        {
            string result = "RESULT: ";
            bool[] matchedInPosition = { false, false, false, false };

            int correctPlacementCount = getCorrectPositionCount(guess, answer, matchedInPosition);
            int correctOutOfPlacementCount = getOutOfPositionCount(guess, answer, matchedInPosition);

            result += String.Concat(Enumerable.Repeat(CORRECT_POSITION_CHAR, correctPlacementCount));
            result += String.Concat(Enumerable.Repeat(INCORRECT_POSITION_CHAR, correctOutOfPlacementCount));

            return result;
        }

        /// <summary>
        /// Counts the number of digits that are correct and in the correct position.
        /// </summary>
        /// <param name="guess">The input provided by the user.</param>
        /// <param name="answer">The randomly generated answer.</param>
        /// <param name="matchedInPosition">Array to keep track of digits in the correct position</param>
        /// <returns>The number of digits that are correct and in the correct position</returns>
        private static int getCorrectPositionCount(string guess, string answer, bool[] matchedInPosition)
        {
            int count = 0;

            for (int i = 0; i < answer.Length; i++)
            {
                if (answer[i] == guess[i])
                {
                    count++;
                    matchedInPosition[i] = true;
                }
            }

            return count;
        }

        /// <summary>
        /// Counts the number of digits that are correct, but in the incorrect position.
        /// </summary>
        /// <param name="guess">The input provided by the user.</param>
        /// <param name="answer">The randomly generated answer.</param>
        /// <param name="matchedInPosition">Array to keep track of digits in the correct position</param>
        /// <returns>The number of digits that are correct, but in the incorrect position.</returns>
        public static int getOutOfPositionCount(string guess, string answer, bool[] matchedInPosition)
        {
            int count = 0;
            bool[] matchedOutOfPosition = new bool[LENGTH_OF_ANSWER];
            matchedInPosition.CopyTo(matchedOutOfPosition, 0);

            for (int i = 0; i < answer.Length; i++)
            {
                if (!matchedInPosition[i])
                {
                    for (int j = 0; j < answer.Length; j++)
                    {
                        if (guess[i] == answer[j] && !matchedOutOfPosition[j])
                        {
                            count++;
                            matchedOutOfPosition[j] = true;
                            break;
                        }
                    }
                }
            }

            return count;
        }
    }
}
