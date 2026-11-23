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