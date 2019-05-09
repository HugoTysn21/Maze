using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class Maze
    {
        private int Size; 
        public Cell[,] Table { get; }
        private int NbGeneratedCell = 0;
        private int NbCell;
        public Cell startCell { get; set; }
        public Cell endCell { get; set; }
        private Random rnd;
        
        // generate a random qui va parcourir toutes les cell du tableau et qui va donner un point ou la cell devra se placer 


        public Maze(int size)
        {
            //je lui précise une taille en int pris du Maze ; ce sera la taille de mon tableau
             
            rnd = new Random();
            Table = new Cell[size, size];
            Size = size;
            NbCell = size * size;


            // je reprend comme condition de boucle la taille du tableau 

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    Table[x, y] = new Cell(x, y);
                }
            }
            GenerateMaze();

            //une fois que la commande generate maze a été generer il remet a 0 les isVisited pour le Path

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    Table[x, y].isVisited = false;
                }
            }
            RandomCoordStartAndEnd(startCell, endCell);
            Path();
            
        }


        /// <summary>
        /// Fonction qui me donne un point de départ de de fin, cela va me servir dans mon chemin
        /// </summary>
        /// <param name="start">Point de départ</param>
        /// <param name="end">Point d'arrivé</param>
        private void RandomCoordStartAndEnd(Cell start, Cell end)
        {
            this.startCell = Table[rnd.Next(Size), rnd.Next(Size)];
            this.endCell = Table[rnd.Next(Size), rnd.Next(Size)];

            //Cell startCell = Table[0,0];
            //Cell yStart = rnd.Next(0, .Count);

            //Cell endCell = Table[9,9];
            //int yEnd = rnd.Next(0, PathStack.Count);

           //startCell = Table[xStart, yStart];
            //endCell = Table[xEnd, yEnd];
        }


        /// <summary>
        /// Cette method génère le labyrinthe, cela se décompose de la sorte :
        /// tant que mon nombre généré de cell n'est pas égale au nombre de cell du tableau
        /// il passe la currentCell a true (visité) et il incrémente puis
        /// sur chaque cell il vérifie si il a des voisins si c'est le cas il push la currentCell dans la 
        /// stack et il prend un random de ma liste de voisins et ce random sera alors la NextCell
        /// il casse le mur de currentCell et celui de NextCell pour pouvoir y acceder
        /// et si il n'a pas de voisins alors il dépile (pop)...
        /// </summary>
        private void GenerateMaze()
        {
            Stack<Cell> GenerateStack = new Stack<Cell>();

            Cell currentCell = Table[0, 0];

            while (NbGeneratedCell < NbCell)
            {
                if (!currentCell.isVisited)
                {
                    NbGeneratedCell++;
                    currentCell.isVisited = true;
                }
                
                List<Cell> Neighbor = GetNeighborCell(currentCell);
                Console.WriteLine(Neighbor.Count);
                if (Neighbor.Count == 0)
                {
                    currentCell = GenerateStack.Pop();
                }
                else
                {
                    GenerateStack.Push(currentCell);

                    int randomCell = rnd.Next(Neighbor.Count);
                    Cell nextCell = Neighbor[randomCell];

                    //si il y a 1 voisins alors je lui delete un walls il va alors ainsi currentCell sera la next cell
                    DeleteWallsBetweenCells(nextCell, currentCell);

                    currentCell = nextCell;
                }
            }

        }


        //je lui set 2 variable de type cell focusCell est egale a la cell ou je suis et next cell est la cell 
        // ou je vais avancer il next uniquement si la condition est ok mais uniquement si je peut avancer 

        /// <summary>
        /// Si la currentCell est égale a la nextCell (en fonction des x et y) alors je passe les murs a false(delete)
        /// </summary>
        /// <param name="nextCell">Variable qui sera la cell ou je vais avancer</param>
        /// <param name="focusCell">Variable qui est la cell ou je suis</param>
        private void DeleteWallsBetweenCells(Cell nextCell, Cell focusCell)
        {
            if (focusCell.x + 1 == nextCell.x)
            {
                //droite
                focusCell.right = false;
                nextCell.left = false;
            }
            else if (focusCell.x - 1 == nextCell.x)
            {
                //gauche
                focusCell.left = false;
                nextCell.right = false;
            }
            else if (focusCell.y + 1 == nextCell.y)
            {
                //en bas
                focusCell.bottom = false;
                nextCell.top = false;
            }
            else if (focusCell.y - 1 == nextCell.y)
            {
                // en haut
                focusCell.top = false;
                nextCell.bottom = false;
            }
        }
        
        /// <summary>
        /// cette methode me retourne une liste de cell de voisins ou les murs ont été détruit 
        /// Par exemple si currentCell.bottom est a false auquel cas il est pas visité
        /// ET que la cell d'en haut n'est pas visité(passé)
        /// alors il ajoute la position de la cell dans la liste
        /// </summary>
        /// <param name="currentCell">currentCell qui est la cellule courante</param>
        /// <returns>retourne une liste de cell</returns>
        private List<Cell> MoveOnNextCellOfPath(Cell currentCell)
        {
            List<Cell> GetNeighborWhereWallsAreDeleted = new List<Cell>();

            if (!currentCell.right && !Table[currentCell.x + 1, currentCell.y].isVisited)
            {
                GetNeighborWhereWallsAreDeleted.Add(Table[currentCell.x + 1, currentCell.y]);
            }

            if (!currentCell.left && !Table[currentCell.x - 1, currentCell.y].isVisited)
            {
                GetNeighborWhereWallsAreDeleted.Add(Table[currentCell.x - 1, currentCell.y]);
            }

            if (!currentCell.top && !Table[currentCell.x, currentCell.y - 1].isVisited)
            {
                GetNeighborWhereWallsAreDeleted.Add(Table[currentCell.x, currentCell.y - 1]);
            }

            if (!currentCell.bottom && !Table[currentCell.x, currentCell.y + 1].isVisited)
            {
                GetNeighborWhereWallsAreDeleted.Add(Table[currentCell.x, currentCell.y + 1]);
            }

            return GetNeighborWhereWallsAreDeleted;
        }

        //faire un fonction at beginning qui va changer en true la var du début 
        // pour lui dire que il a trouvé le point et faire changer le end en true faire un 
        // if dans le while qui verifie en 1 ligne si end est egale a start auquel cas (start = true 
        // peut on verifier si ma stack contient la cell au coord de end (random ou pas random) alors cela veut dire qu'il l'as parcouru ou est dessus
        // si ma stack contient end alors end passe a true et on n'avance plus car la condition du while n'est plus respecté 
        //2eme méthode
        //faire var bool ispassed a chaque fois que currentCell passe sur une cell sa case passe a true(on la marque en tant que passé )
        // si currentCell est égale a end alors il s'arrete`


        /// <summary>
        /// Cette methode definie : tant que ma cellule n'a pas atteint ma cellule de fin alors
        /// on creer une liste qui aura la liste de cell ou les murs sont detruit 
        /// et la cellule courante est visité
        /// puis : SI il y a aucun murs ouvert hormis celui d'ou l'on vient cela veut dire
        /// que la currentCell n'est pas dans le chemin et donc on dépile et on re-test
        /// sinnon on prend un random des murs qui sont ouvert et on donne a NextCell le random de ce dernier
        /// ducoup NextCell est donc dans le chemin -> currentCell devient donc NextCell 
        /// </summary> 
        private void Path()
        {
            Cell currentCell = startCell;
            Stack<Cell> PathStack = new Stack<Cell>();

            while (currentCell != endCell)
            {
                List<Cell> OpenAdjacentCells = MoveOnNextCellOfPath(currentCell);
                currentCell.isVisited = true;

                if (OpenAdjacentCells.Count == 0)
                {
                    currentCell.isInPath = false;
                    currentCell = PathStack.Pop();
                }
                else
                {
                    PathStack.Push(currentCell);
                    int rndCells = rnd.Next(0, OpenAdjacentCells.Count);
                    Cell NextCell = OpenAdjacentCells[rndCells];

                    NextCell.isInPath = true;
                    currentCell = NextCell;
                }
            }
        }

            /// <summary>
            /// conditions pour définir les bordures du maze 
            /// </summary>
            /// <param name="currentCell"></param>
            /// <returns>retourne une liste voisins</returns>
        private List<Cell> GetNeighborCell(Cell currentCell)
        {
            List<Cell> Adjacent = new List<Cell>();

            //verifie si la cellule target n'est pas au bord des coord x-0
            //puis vérifie si elle a été visité
            //cell est la position de x - 1 si il n'est pas au bord et on lui indique la pos de y
            //continuer pour les autres conditions x +1...

            //ordonné x
            if (currentCell.x - 1 > 0)
            {
                Cell cell = Table[currentCell.x - 1, currentCell.y];
                if (!cell.isVisited)
                {
                    Adjacent.Add(cell);
                }
            }
            if (currentCell.x + 1 < Size)
            {
                Cell cell = Table[currentCell.x + 1, currentCell.y];
                //si isVisited est egale a false donc pas visité 
                if (!cell.isVisited)
                {
                    Adjacent.Add(cell);
                }
            }
            // abcisse Y
            if (currentCell.y - 1 > 0)
            {
                Cell cell = Table[currentCell.x, currentCell.y - 1];
                if (!cell.isVisited)
                {
                    Adjacent.Add(cell);
                }
            }
            if (currentCell.y + 1 < Size)
            {
                Cell cell = Table[currentCell.x, currentCell.y + 1];
                if (!cell.isVisited)
                {
                    Adjacent.Add(cell);
                }
            }
            return Adjacent;
        }

        public override string ToString()
        {
            string returnString = "";

            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    returnString += $"{Table[x, y]} ";
                }
                returnString += "\n";
            }

            return returnString;
        }
    }
}
