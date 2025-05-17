namespace CerebroXMen.Application.Services;

public class MutantDetectorService : IMutantDetectorService
{
    public bool IsMutant(string[] dna)
    {
        if (dna == null || dna.Length == 0)
            return false;

        int n = dna.Length;
        for (int i = 0; i < n; i++)
        {
            if (dna[i].Length != n)
                throw new ArgumentException("DNA must be a square matrix");
        }

        int mutantSequences = 0;

        // Revisamos filas, columnas, diagonales principales y secundarias
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (CheckSequence(dna, i, j, 0, 1)) mutantSequences++;     // Horizontal
                if (CheckSequence(dna, i, j, 1, 0)) mutantSequences++;     // Vertical
                if (CheckSequence(dna, i, j, 1, 1)) mutantSequences++;     // Diagonal \
                if (CheckSequence(dna, i, j, 1, -1)) mutantSequences++;    // Diagonal /

                if (mutantSequences >= 2)
                    return true; // Mutante si tiene 2 o más secuencias iguales consecutivas
            }
        }
        return false;
    }

    private bool CheckSequence(string[] dna, int row, int col, int rowDir, int colDir)
    {
        int n = dna.Length;
        char? firstChar = null;
        for (int k = 0; k < 4; k++)
        {
            int r = row + rowDir * k;
            int c = col + colDir * k;

            if (r < 0 || r >= n || c < 0 || c >= n)
                return false;

            char currentChar = dna[r][c];
            if (firstChar == null)
                firstChar = currentChar;
            else if (currentChar != firstChar)
                return false;
        }
        return true;
    }
}