using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SS;
using SpreadsheetUtilities;
using System.Text.RegularExpressions;

namespace SS
{
    public class Spreadsheet : AbstractSpreadsheet
    {
        /// <summary>
        /// This will contain all the data of dependencies in 
        /// my spreadsheet.
        /// </summary>
        private DependencyGraph dependGraph;

        /// <summary>
        /// Need to have something represent the spreadsheet
        /// </summary>
        private Dictionary<string, Cell> spreadsheet;

        /// <summary>
        /// Constructor
        /// </summary>
        public Spreadsheet()
        {
            dependGraph = new DependencyGraph();
            spreadsheet = new Dictionary<string,Cell>;
        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            foreach (KeyValuePair<string, Cell> cell in spreadsheet)
            {
                if (cell.Value.Contents != string.Empty)
                {
                    yield return cell.Key;

                }
            }
            
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        public override object GetCellContents(String name)
        {
            if (name == null)
            {
                throw new InvalidNameException();
            }
            else
            {
                return spreadsheet[name].Contents;
            }
        }


        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<String> SetCellContents(String name, double number)
        {
            if (name == null || !Regex.IsMatch(name, @"[a-zA-Z_]+(?: [a-zA-Z_]|\d)8"))
            {
                throw new InvalidNameException();
            }
            else if(spreadsheet.ContainsKey(name))
            {
                spreadsheet[name].Contents = number;
            }
            HashSet<string> dependents;

            return dependents = new HashSet<string>(GetCellsToRecalculate(name));
        }

        /// <summary>
        /// If text is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes text.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<String> SetCellContents(String name, String text)
        {
            if (text == null)
            {
                throw new ArgumentNullException();
            }
            if (name == null || !Regex.IsMatch(name, @"[a-zA-Z_]+(?: [a-zA-Z_]|\d)8"))
            {
                throw new InvalidNameException();
            }
            else
            {
                spreadsheet[name].Contents = text;
            }
            HashSet<string> dependents = new HashSet<string>(GetCellsToRecalculate(name));

            return dependents;
        }

        /// <summary>
        /// If the formula parameter is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException.  (No change is made to the spreadsheet.)
        /// 
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// Set consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<String> SetCellContents(String name, Formula formula)
        {
            if (formula == null)
            {
                throw new ArgumentNullException();
            }
            if (name == null || !Regex.IsMatch(name, @"[a-zA-Z_]+(?: [a-zA-Z_]|\d)8"))
            {
                throw new InvalidNameException();
            }
            else
            {
                spreadsheet[name].Contents = formula;
            }
            HashSet<string> dependents = new HashSet<string>(GetCellsToRecalculate(name));

            return dependents;
        }


        /// <summary>
        /// If name is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name isn't a valid cell name, throws an InvalidNameException.
        /// 
        /// Otherwise, returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// 
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and C1
        /// </summary>
        protected override IEnumerable<String> GetDirectDependents(String name)
        {
            if (name == null)
            {
                throw new ArgumentNullException();
            }

            if (name == null || !Regex.IsMatch(name, @"[a-zA-Z_]+(?: [a-zA-Z_]|\d)8"))
            {
                throw new InvalidNameException();
            }
            return dependGraph.GetDependents(name);
        }


        /// <summary>
        /// Requires that names be non-null.  Also requires that if names contains s,
        /// then s must be a valid non-null cell name.
        /// 
        /// If any of the named cells are involved in a circular dependency,
        /// throws a CircularException.
        /// 
        /// Otherwise, returns an enumeration of the names of all cells whose values must
        /// be recalculated, assuming that the contents of each cell named in names has changed.
        /// The names are enumerated in the order in which the calculations should be done.  
        /// 
        /// For example, suppose that 
        /// A1 contains 5
        /// B1 contains 7
        /// C1 contains the formula A1 + B1
        /// D1 contains the formula A1 * C1
        /// E1 contains 15
        /// 
        /// If A1 and B1 have changed, then A1, B1, and C1, and D1 must be recalculated,
        /// and they must be recalculated in either the order A1,B1,C1,D1 or B1,A1,C1,D1.
        /// The method will produce one of those enumerations.
        /// 
        /// PLEASE NOTE THAT THIS METHOD DEPENDS ON THE ABSTRACT METHOD GetDirectDependents.
        /// IT WON'T WORK UNTIL GetDirectDependents IS IMPLEMENTED CORRECTLY.
        /// </summary>
        protected IEnumerable<String> GetCellsToRecalculate(ISet<String> names)
        {
            LinkedList<String> changed = new LinkedList<String>();
            HashSet<String> visited = new HashSet<String>();
            foreach (String name in names)
            {
                if (!visited.Contains(name))
                {
                    Visit(name, name, visited, changed);
                }
            }
            return changed;
        }


        /// <summary>
        /// A convenience method for invoking the other version of GetCellsToRecalculate
        /// with a singleton set of names.  See the other version for details.
        /// </summary>
        protected IEnumerable<String> GetCellsToRecalculate(String name)
        {
            return GetCellsToRecalculate(new HashSet<String>() { name });
        }


        /// <summary>
        /// A helper for the GetCellsToRecalculate method.
        /// 
        ///   -- You should fully comment what is going on below --
        /// </summary>
        private void Visit(String start, String name, ISet<String> visited, LinkedList<String> changed)
        {
            visited.Add(name);
            foreach (String n in GetDirectDependents(name))
            {
                if (n.Equals(start))
                {
                    throw new CircularException();
                }
                else if (!visited.Contains(n))
                {
                    Visit(start, n, visited, changed);
                }
            }
            changed.AddFirst(name);
        }
    }

    /// <summary>
    /// This class will be good to have when dealing with cells.
    /// </summary>
    private class Cell
    {
        /// <summary>
        /// this will hold the name of the cell
        /// </summary>
        public String cellName { get; private set; }

        /// <summary>
        /// this will hold the contents of a cell,
        /// whether that be a double, string or a formula.
        /// </summary>
        public object Contents { get; set; }

        public Cell(string name, object stuffInside)
        {
            cellName = name;
            Contents = stuffInside;
        }
    }
}
