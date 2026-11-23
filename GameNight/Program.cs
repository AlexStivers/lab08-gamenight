/*
 * Name:        Alex Stivers
 * Course:      CSCI 1250
 * Assignment:  Lab 08, Game Night
 * Date:        November 22, 2026
 * Description: Keeps a game night leaderboard in three text files. Shows the
 *              season so far, records tonight's points, saves the new totals,
 *              and adds tonight's result to a history that grows every week.
 */

const string PLAYERS_FILE = "players.txt";
const string POINTS_FILE = "points.txt";
const string HISTORY_FILE = "history.txt";
const int MAX_POINTS = 20;

// Look for the data files next to Program.cs, however the program is started.
string projectFolder = Path.GetFullPath(
    Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
Directory.SetCurrentDirectory(projectFolder);

Console.WriteLine("=== GAME NIGHT LEADERBOARD ===");

// Part 3 (top): The History: read past nights from history.txt and determine tonight's night number.
string[] historyLines = File.ReadAllLines(HISTORY_FILE);
if (historyLines.Length == 0)
{
    Console.WriteLine("No nights played yet.");
}
else
{
    Console.WriteLine($"Nights played: {historyLines.Length}");
    // foreach: every recorded night line is printed in order without needing index control
    foreach (string line in historyLines)
    {
        Console.WriteLine($"  {line}");
    }
}

int tonightNumber = historyLines.Length + 1;
Console.WriteLine($"Tonight is Night {tonightNumber}.");
Console.WriteLine();

// Part 1: The Standings - read player names and points, print the table, leader, and season total.
string[] players = File.ReadAllLines(PLAYERS_FILE);
string[] points = File.ReadAllLines(POINTS_FILE);

Console.WriteLine($"{"PLAYER".PadRight(11)}POINTS");

// for: player names and point strings live in parallel arrays and require a shared index.
for (int i = 0; i < players.Length; i++)
{
    Console.WriteLine($"{players[i].PadRight(11)}{points[i]}");
}

int leader = LeaderIndex(points);
Console.WriteLine($"Leader: {players[leader]} with {points[leader]}");

int seasonTotal = TotalPoints(points);
Console.WriteLine($"Season total: {seasonTotal}");

// Part 2: Game Night - collect valid nightly scores, update season totals, name tonight's winner, and save.
int bestTonight = -1;
string winnerTonight = "";

// for: each player's nightly score must be paired with their name and total at index i.
for (int i = 0; i < players.Length; i++)
{
    Console.Write($"Points tonight for {players[i]}: ");
    int tonight = Convert.ToInt32(Console.ReadLine());

    //while: input validation repeats an unknown number of times until the score is in range.
    while (tonight < 0 || tonight > MAX_POINTS)
    {
        Console.WriteLine($"Points must be from 0 to {MAX_POINTS}.");
        Console.Write($"Points tonight for {players[i]}: ");
        tonight = Convert.ToInt32(Console.ReadLine());
    }

    int season = Convert.ToInt32(points[i]) + tonight;
    points[i] = season.ToString();

    if (tonight > bestTonight)
    {
        bestTonight = tonight;
        winnerTonight = players[i];
    }
}

Console.WriteLine($"Tonight's winner: {winnerTonight} with {bestTonight}");

File.WriteAllLines(POINTS_FILE, points);
Console.WriteLine("Saved points.txt.");

// Part 3 (bottom): The History: build tonight's history line and append it to history.txt.
string[] entry = { HistoryEntry(tonightNumber, winnerTonight, bestTonight) };
File.AppendAllLines(HISTORY_FILE, entry);
Console.WriteLine($"Added to history.txt: {entry[0]}");

// Hands back the index of the highest score in the points array.
static int LeaderIndex(string[] points)
{
    int bestIndex = 0;
    int bestScore = Convert.ToInt32(points[0]);

    // for: comparing each score against the current best requires index access to track position.
    for (int i = 1; i < points.Length; i++)
    {
        int current = Convert.ToInt32(points[i]);
        if (current > bestScore)
        {
            bestScore = current;
            bestIndex = i;
        }
    }
    
    return bestIndex;
}

// Hands back the sum of all scores in the points array.
static int TotalPoints(string[] points)
{
    int total = 0;

    // foreach: summing every score only needs each value in order without modifying or indexing by position.
    foreach (string entry in points)
    {
        total += Convert.ToInt32(entry);
    }

    return total;
}

// Hands back the formatted history line for a completed game night.
static string HistoryEntry(int night, string winner, int points)
{
    return $"Night {night}: {winner} won with {points} points";
}