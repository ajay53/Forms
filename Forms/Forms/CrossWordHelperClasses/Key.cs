using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace CrossWordHelper
{
    public class Key : Button
    {
        static char axis = 'a';
        List<long> ids = new List<long>();
        static List<object> selectedButtonList = new List<object>();
        public event EventHandler StatusChanged;
        StringBuilder selectedWord = new StringBuilder(string.Empty);

        public Key()
        {
            TouchEffect effect = new TouchEffect();
            effect.TouchAction += OnTouchEffectAction;
            Effects.Add(effect);
        }

        public int KeyNumber { set; get; }

        public bool IsButtonPressed { private set; get; }

        protected Color UpColor { set; get; }

        protected Color DownColor { set; get; }

        void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {
            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    Console.WriteLine("Key");
                    //AddToList(args.Id);
                    AddButtonToList(sender);
                    break;

                case TouchActionType.Entered:
                    //if (args.IsInContact)
                    //{
                    //    AddToList(args.Id);
                    //}
                    AddButtonToList(sender);
                    break;

                case TouchActionType.Moved:
                    break;

                case TouchActionType.Released:
                    validateWord();
                    //SetUpColor();
                    break;
                case TouchActionType.Exited:
                    //RemoveFromList(args.Id);
                    break;
            }
        }

        private void validateWord()
        {
            //add each character to selectedWord(StringBuilder) 
            foreach (var buttonObj in selectedButtonList)
            {
                GridButton gridButton = (GridButton)buttonObj;
                selectedWord.Append(gridButton.Text);
                Console.WriteLine($"row: {gridButton.row}---- column: {gridButton.column}");
            }
            Console.WriteLine($"result: {selectedWord}");

            selectedButtonList.Clear();
            selectedWord.Clear();
        }

        void AddToList(long id)
        {
            if (!ids.Contains(id))
            {
                ids.Add(id);
            }

            CheckList();
        }

        void RemoveFromList(long id)
        {
            if (ids.Contains(id))
            {
                ids.Remove(id);
            }

            CheckList();
        }

        void CheckList()
        {
            if (IsButtonPressed != ids.Count > 0)
            {
                IsButtonPressed = ids.Count > 0;
                BackgroundColor = IsButtonPressed ? DownColor : UpColor;
                StatusChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        void SetUpColor()
        {
            foreach (var buttonObj in selectedButtonList)
            {
                ((Button)buttonObj).BackgroundColor = Color.LightBlue;
            }
            selectedButtonList.Clear();
        }

        void SetDownColor(object buttonObj)
        {
            ((Button)buttonObj).BackgroundColor = Color.LightGreen;
        }

        //allows every element on correct axis.
        void AddButtonToList(object buttonObj)
        {
            //make sure next button/character is on the same axis.
            //check for result word in output.
            Console.WriteLine($"result: {selectedWord}");
            GridButton currentElement = (GridButton)buttonObj;
            int rowDiff, columnDiff;

            //store 1st two element/button.
            if (selectedButtonList.Count < 2)
            {
                selectedButtonList.Add(buttonObj);
                Console.WriteLine("one");
            }

            //storing next element/button on drag.
            else if (selectedButtonList.Count == 2)
            {
                Console.WriteLine("two");
                GridButton firstElement = (GridButton)selectedButtonList.First();
                GridButton secondElement = (GridButton)selectedButtonList.ElementAt(1);

                rowDiff = Math.Abs(firstElement.row - secondElement.row);
                columnDiff = Math.Abs(firstElement.column - secondElement.column);

                //for elements at x-axis, y-axis and z-axis respectively.
                if (currentElement.row == firstElement.row)
                {
                    axis = 'x';
                    Console.WriteLine($"axis :{axis}");
                    
                }
                else if (currentElement.column == firstElement.column)
                {
                    axis = 'y';
                    Console.WriteLine($"axis :{axis}");
                    
                }
                else if (rowDiff == columnDiff)
                {
                    axis = 'z';
                    Console.WriteLine($"axis :{axis}");
                    
                }
            }

            if (axis != 'a')
            {
                GridButton startElement = (GridButton)selectedButtonList.ElementAt(0);
                switch (axis)
                {
                    case 'x':
                        Console.WriteLine("axisXX");
                        if (currentElement.row == startElement.row)
                        {
                            selectedButtonList.Add(buttonObj);
                        }
                        break;
                    case 'y':
                        Console.WriteLine("axisYY");
                        if (currentElement.column == startElement.column)
                        {
                            selectedButtonList.Add(buttonObj);
                        }
                        break;
                    case 'z':
                        Console.WriteLine("axisZZ");
                        rowDiff = Math.Abs(startElement.row - currentElement.row);
                        columnDiff = Math.Abs(startElement.column - currentElement.column);

                        if (rowDiff == columnDiff)
                        {
                            selectedButtonList.Add(buttonObj);
                        }
                        break;
                    default:
                        Console.WriteLine($"list: {selectedButtonList.Count}");
                        break;
                }
            }
            //if (selectedButtonList.Count == 2)
            //{
            //    GridButton firstElement = (GridButton)selectedButtonList.First();
            //    rowDiff = Math.Abs(currentButton.row - firstElement.row);
            //    columnDiff = Math.Abs(currentButton.column - firstElement.column);

            //    //for elements at x-axis, y-axis and z-axis respectively.
            //    if (currentButton.row == firstElement.row || currentButton.column == firstElement.column || rowDiff == columnDiff)
            //    {
            //        selectedButtonList.Add(buttonObj);
            //    }

            //    return;
            //}


            
        }

    }
}
