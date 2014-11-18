using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{

    public class Parser
    {

        List<Skaner.Lexems> Lexems;
        Skaner.Lexems CurrentLexem;
        int count;
        Stack<int> States=new Stack<int>();
        int StateFlag;//0-common 1-then 2-else 
        public Parser(List<Skaner.Lexems> Lexems)
        {
            this.Lexems = new List<Skaner.Lexems>(Lexems);
            count = 0;
            StateFlag = 0;
            
        }
        public Parser(Parser t)
        {
            Lexems = t.Lexems;
            CurrentLexem = t.CurrentLexem;
            count = t.count;
        }
        List<Skaner.Lexems> GetLine()
        {
            List<Skaner.Lexems> t = new List<Skaner.Lexems>();
            while (GetNextLexem().LexemCode != 31)
                t.Add(CurrentLexem);
            return t;
        }


        Skaner.Lexems TryNextLexem()
        {
            System.Diagnostics.Debug.WriteLine("count=" + count.ToString() + "|" + Lexems.Count.ToString());
            if (count >= Lexems.Count)
            { return new Skaner.Lexems() { LexemCode = -1 }; }
            else return Lexems[count];
        }
        Skaner.Lexems GetNextLexem()
        {
            if (count >= Lexems.Count)
                return null;
            else
            {
                CurrentLexem = Lexems[count++];
                return CurrentLexem;
            }
        }
        public bool check()
        {

            if (TryNextLexem().LexemCode == 1)
            {
                GetNextLexem();
                if (TryNextLexem().LexemCode == 30)
                {
                    GetNextLexem();
                    if (TryNextLexem().LexemCode == 50)
                    {
                        GetNextLexem();
                        if (TryNextLexem().LexemCode == 31)
                        {
                            GetNextLexem();
                            if (TryNextLexem().LexemCode == 26)
                            {
                                GetNextLexem();
                                if (TryNextLexem().LexemCode == 31)
                                {
                                    GetNextLexem();
                                    SkipEnters();
                                    if (IsElemList())
                                    {
                                        SkipEnters();
                                        if (TryNextLexem().LexemCode == 27)
                                        {
                                            GetNextLexem();

                                            if (TryNextLexem().LexemCode == -1)
                                            {
                                                return true;////////////////
                                            }
                                            else
                                            {
                                                throw new Exceptions.MyException("Error! After the last scope can't be any text  Line:" + CurrentLexem.RowNumber.ToString());
                                            }
                                        }
                                        else
                                        {
                                            System.Diagnostics.Debug.WriteLine(TryNextLexem().LexemCode);
                                            throw new Exceptions.MyException("Error! make sure you closed the scope  Line:" + CurrentLexem.RowNumber);
                                        }
                                    }

                                    else
                                    {
                                        throw new Exceptions.MyException("Error! Elem List expected Line:" + CurrentLexem.RowNumber);
                                    }
                                }
                                else
                                {

                                    throw new Exceptions.MyException("Error! make sure you made new line Line:2");
                                }
                            }
                            else
                            {

                                throw new Exceptions.MyException("Error! \"/\" expected Line:2");
                            }
                        }
                        else
                        {

                            throw new Exceptions.MyException("Error! make sure you made new line Line:1");
                        }
                    }
                    else
                    {

                        throw new Exceptions.MyException("Error! program name expected  Line:1");
                    }
                }
                else
                {

                    throw new Exceptions.MyException("Error! \":\" expected  Line:1");
                }
            }
            else
            {

                throw new Exceptions.MyException("Error! init expected  Line:1");

            }

            return true;

        }
        private void SkipEnters()
        {
            while (TryNextLexem().LexemCode == 31)
                GetNextLexem();
        }
        private bool IsElemList()
        {

            if (IsElem())
            {
                if (TryNextLexem().LexemCode == 31)
                {
                    GetNextLexem();
                    SkipEnters();
                    if (IsElemList())
                    {

                        return true;
                    }
                    else
                    {

                        return true;
                    }
                }
                else throw new Exceptions.MyException("Error! make sure you made new line  Line:" + CurrentLexem.RowNumber);
            }
            else
            {
                if (TryNextLexem().LexemCode == 10 && StateFlag == 1)
                    return true;
                else
                    if (TryNextLexem().LexemCode == 11 && StateFlag == 2)
                        return true;
                    else
                        if (TryNextLexem().LexemCode == 27)
                            return true;
                        else
                            if (TryNextLexem().LexemCode == -1)
                                return true;
                        else throw new Exceptions.MyException("Wrong operator Line:" + (CurrentLexem.RowNumber + 1).ToString());
            }
        }



        private bool IsElem()
        {
            if (TryNextLexem().LexemCode == 26)
            {
                GetNextLexem();

                if (TryNextLexem().LexemCode == 31)
                {
                    GetNextLexem();
                    SkipEnters();
                    if (IsElemList())
                    {
                        SkipEnters();
                        if (TryNextLexem().LexemCode == 27)
                        {
                            GetNextLexem();
                            //SkipEnters();
                            return true;
                        }
                        else
                        {
                            throw new Exceptions.MyException("Error! make sure you closed the scope  Line:" + CurrentLexem.RowNumber);
                        }
                    }
                    else
                    {
                        throw new Exceptions.MyException("Error! Elem list expected  Line:" + CurrentLexem.RowNumber);
                    }
                }
                else
                {
                    throw new Exceptions.MyException("Error! make sure you made new line  Line:" + CurrentLexem.RowNumber);
                }
            }
            else

                if (IsLabel() || IsOgol() || IsOper())
                {
                    return true;
                }
                else return false;
        }

        private bool IsLabel()
        {


            if (TryNextLexem().type == 3)
            {
                GetNextLexem();

                if (TryNextLexem().LexemCode == 30)
                {
                    GetNextLexem();
                    return true;
                }
                else
                {
                    throw new Exceptions.MyException("Error! Make sure you used syntaxis: label's name : Line:" + CurrentLexem.RowNumber);
                }

            }
            else return false;

        }


        private bool IsOper()
        {

            if (IsPrysv() || IsOutput() || IsInput() || IsGoto() || IsCondition() || isLoop())
            {

                return true;
            }
            else
            {
                return false;
            };
        }

        private bool IsGoto()
        {

            if (TryNextLexem().LexemCode == 12)
            {
                GetNextLexem();
                if (TryNextLexem().type == 5)
                {
                    GetNextLexem();
                    return true;
                }
                else
                {
                    throw new Exceptions.MyException("Error! Make sure you used syntaxis: goto label Line:" + CurrentLexem.RowNumber);
                };

            }
            return false;
        }

        private bool IsPrysv()
        {

            if (TryNextLexem().LexemCode == 50)
            {
                GetNextLexem();
                if (TryNextLexem().LexemCode == 13)
                {
                    GetNextLexem();
                    if (IsArythm())
                        return true;
                    else throw new Exceptions.MyException("Error! arythm expression expected Line:" + CurrentLexem.RowNumber);
                }
                else throw new Exceptions.MyException("Error! Make sure you used syntaxis: variable=arythm exp. Line:" + CurrentLexem.RowNumber);
            }
            return false;

        }

        private bool IsArythm()
        {
            if (TryNextLexem().LexemCode == 24)
            {
                GetNextLexem();
                if (IsArythm())
                    return true;
                else throw new Exceptions.MyException("Error! wrong arytmetical expression Line:" + CurrentLexem.RowNumber);
            }
            else
            {
                if (IsDodanok())
                {
                    if (TryNextLexem().LexemCode == 23 || TryNextLexem().LexemCode == 24)
                    {
                        GetNextLexem();
                        if (IsArythm())
                            return true;
                        else throw new Exceptions.MyException("Error! wrong arytmetical expression Line:" + CurrentLexem.RowNumber);
                    }
                    else return true;
                }
                else throw new Exceptions.MyException("Error! wrong arytmetical expression Line:" + CurrentLexem.RowNumber);
            }

        }

        private bool IsDodanok()
        {
            if (IsMnog())
            {
                if (TryNextLexem().LexemCode == 25 || TryNextLexem().LexemCode == 26)
                {
                    GetNextLexem();
                    if (IsDodanok())
                        return true;
                    else throw new Exceptions.MyException("Error! wrong arytmetical expression Line:" + CurrentLexem.RowNumber);
                }
                else return true;
            }
            else throw new Exceptions.MyException("Error! wrong arytmetical expression Line:" + CurrentLexem.RowNumber);
        }

        private bool IsMnog()
        {
            if (TryNextLexem().LexemCode == 50 || TryNextLexem().LexemCode == 51)
            {
                GetNextLexem();
                return true;
            }
            else
            {
                if (TryNextLexem().LexemCode == 28)
                {
                    GetNextLexem();
                    if (IsArythm())
                    {
                        if (TryNextLexem().LexemCode == 29)
                        {
                            GetNextLexem();
                            return true;
                        }
                        else throw new Exceptions.MyException("Error! wrong arytmetical expression Line:" + CurrentLexem.RowNumber);
                    }
                    else throw new Exceptions.MyException("Error! wrong arytmetical expression Line:" + CurrentLexem.RowNumber);

                }
                else throw new Exceptions.MyException("Error! wrong arytmetical expression Line:" + CurrentLexem.RowNumber);

            }
        }

        private bool IsOutput()
        {

            if (TryNextLexem().LexemCode == 4)
            {

                GetNextLexem();
                if (TryNextLexem().LexemCode == 28)
                {
                    GetNextLexem();
                    if (IsArythm())
                    {
                        if (TryNextLexem().LexemCode == 29)
                        {
                            GetNextLexem();
                            return true;
                        }
                        else
                        {
                            throw new Exceptions.MyException("Error! Right scope expected Line:" + CurrentLexem.RowNumber);
                        }
                    }
                    else
                    {
                        throw new Exceptions.MyException("Error! Id list expected Line:" + CurrentLexem.RowNumber);
                    }


                }
                else
                {
                    throw new Exceptions.MyException("Error! Left scope expected Line:" + CurrentLexem.RowNumber);
                }
            }
            else
            {
                return false;
            };
        }

        private bool IsInput()
        {
            if (TryNextLexem().LexemCode == 5)
            {
                GetNextLexem();
                if (TryNextLexem().LexemCode == 28)
                {
                    GetNextLexem();
                    if (IsSpysId())
                    {
                        if (TryNextLexem().LexemCode == 29)
                        {
                            GetNextLexem();
                            return true;
                        }
                        else
                        {
                            throw new Exceptions.MyException("Error! identificator's or right scope expected Line:" + CurrentLexem.RowNumber);
                        }
                    }
                    else
                    {
                        if (TryNextLexem().LexemCode == 29)
                        {
                            GetNextLexem();
                            return true;
                        }
                        else
                        {
                            throw new Exceptions.MyException("Error! identificator's or right scope expected Line:" + CurrentLexem.RowNumber);
                        }
                    }
                }
                else
                {
                    throw new Exceptions.MyException("Error! Left scope expected Line:" + CurrentLexem.RowNumber);
                }
            }
            else
            {
                return false;
            }

        }
        private bool isLoop()
        {
            if (TryNextLexem().LexemCode == 6)
            {
                GetNextLexem();
                if (TryNextLexem().LexemCode == 28)
                {
                    GetNextLexem();
                    if (IsLogic())
                    {
                        if (TryNextLexem().LexemCode == 29)
                        {
                            GetNextLexem();
                            if (TryNextLexem().LexemCode == 7)
                            {
                                GetNextLexem();
                                if (TryNextLexem().LexemCode == 31)
                                {
                                    GetNextLexem();

                                    if (IsElem())
                                        return true;
                                    else
                                    {
                                        GetNextLexem();
                                        throw new Exceptions.MyException("Error! Wrong operator after while,operator is expected Line:" + CurrentLexem.RowNumber);
                                    }
                                }

                                else throw new Exceptions.MyException("Error! new line expected Line:" + CurrentLexem.RowNumber);
                            }
                            else throw new Exceptions.MyException("Error! do expected Line:" + CurrentLexem.RowNumber);
                        }
                        else throw new Exceptions.MyException("Error! right scope expected Line:" + CurrentLexem.RowNumber);
                    }
                    else throw new Exceptions.MyException("Error! logic exp expected Line:" + CurrentLexem.RowNumber);
                }
                else throw new Exceptions.MyException("Error! Left scope expected Line:" + CurrentLexem.RowNumber);
            }
            else
                return false;
        }
        private bool IsCondition()
        {
            States.Push(StateFlag);   
            if (TryNextLexem().LexemCode == 8)
            {
                GetNextLexem();
                if (TryNextLexem().LexemCode == 28)
                {
                    GetNextLexem();
                    if (IsLogic())
                    {
                        if (TryNextLexem().LexemCode == 29)
                        {
                            GetNextLexem();
                            if (TryNextLexem().LexemCode == 9)
                            {
                                GetNextLexem();
                                if (TryNextLexem().LexemCode == 31)
                                {
                                    GetNextLexem();
                                    StateFlag = 1;
                                    SkipEnters();
                                    if (IsElemList())
                                    {
                                        
                                        SkipEnters();
                                        if (TryNextLexem().LexemCode == 10)
                                        {
                                            GetNextLexem();

                                            if (TryNextLexem().LexemCode == 31)
                                            {
                                                GetNextLexem();
                                                StateFlag = 2;
                                                SkipEnters();
                                                if (IsElemList())
                                                {
                                                    SkipEnters();
                                                    
                                                    if (TryNextLexem().LexemCode == 11)
                                                    {
                                                        GetNextLexem();
                                                        StateFlag = States.Pop();
                                                        return true;

                                                    }
                                                    else
                                                        throw new Exceptions.MyException("Error! endif expected Line:" + CurrentLexem.RowNumber);
                                                }
                                                else
                                                    throw new Exceptions.MyException("Error! wrong elem list Line:" + CurrentLexem.RowNumber);
                                            }
                                            else throw new Exceptions.MyException("Error! new line expected Line:" + CurrentLexem.RowNumber);
                                        }
                                        else
                                            throw new Exceptions.MyException("Error! else expected Line:" + CurrentLexem.RowNumber);
                                    }
                                    else
                                        throw new Exceptions.MyException("Error! wrong elem list Line:" + CurrentLexem.RowNumber);
                                }
                                else
                                    throw new Exceptions.MyException("Error! new line expected Line:" + CurrentLexem.RowNumber);
                            }
                            else
                                throw new Exceptions.MyException("Error! then expected Line:" + CurrentLexem.RowNumber);
                        }
                        else throw new Exceptions.MyException("Error! right scope expected Line:" + CurrentLexem.RowNumber);
                    }
                    else
                        throw new Exceptions.MyException("Error! logic exp expected Line:" + CurrentLexem.RowNumber);
                }
                else
                    throw new Exceptions.MyException("Error! Left scope expected Line:" + CurrentLexem.RowNumber);
            }
            else {
                States.Pop();
                return false;
            }
             
           
        }

        private bool IsLogic()
        {

            if (IsLogDodanok())//dodanok
            {
                if (TryNextLexem().LexemCode == 21)
                {
                    GetNextLexem();
                    if (IsLogic())
                        return true;
                    else throw new Exceptions.MyException("Error! wrong logic expression Line:" + CurrentLexem.RowNumber);
                }
                else return true;
            }
            else throw new Exceptions.MyException("Error! wrong logic expression Line:" + CurrentLexem.RowNumber);


        }

        private bool IsLogDodanok()
        {
            if (IsLogMnog())
            {
                if (TryNextLexem().LexemCode == 20)
                {
                    GetNextLexem();
                    if (IsLogDodanok())
                        return true;
                    else throw new Exceptions.MyException("Error! wrong arytmetical expression Line:" + CurrentLexem.RowNumber);
                }
                else return true;
            }
            else throw new Exceptions.MyException("Error! wrong arytmetical expression Line:" + CurrentLexem.RowNumber);
        }

        private bool IsLogMnog()
        {



            if (TryNextLexem().LexemCode == 22)
            {

                GetNextLexem();
                if (IsLogMnog())
                    return true;
                else
                {
                    throw new Exceptions.MyException("Error! wrong logical expression Line:" + CurrentLexem.RowNumber);
                }
            }
            else
            {
                if (TryNextLexem().LexemCode == 28)
                {
                    GetNextLexem();
                    if (IsLogic())
                    {
                        if (TryNextLexem().LexemCode == 29)
                        {
                            GetNextLexem();
                            return true;
                        }
                        else throw new Exceptions.MyException("Error! wrong logical expression Line:" + CurrentLexem.RowNumber);
                    }
                    else throw new Exceptions.MyException("Error! wrong logical expression Line:" + CurrentLexem.RowNumber);

                }
                else
                    if (IsArythm())
                    {
                        if (isLogSign())
                        {
                            if (IsArythm())
                            {
                                return true;
                            }
                            else
                                throw new Exceptions.MyException("Error! wrong arythm exp in log exp Line:" + CurrentLexem.RowNumber);
                        }
                        else
                            throw new Exceptions.MyException("Error! wrong logical sign Line:" + CurrentLexem.RowNumber);

                    }
                    else throw new Exceptions.MyException("Error! wrong logical expression Line:" + CurrentLexem.RowNumber);

            }
        }

        private bool isLogSign()
        {
            if (TryNextLexem().LexemCode <= 19 && TryNextLexem().LexemCode >= 14)
            {
                GetNextLexem();
                return true;
            }
            return false;
        }
        private bool IsOgol()
        {

            if ((TryNextLexem().LexemCode == 2 || TryNextLexem().LexemCode == 3))
            {
                GetNextLexem();
                if (IsSpysId())
                {
                    return true;
                }
                else
                {
                    throw new Exceptions.MyException("Error! Identificator list expected Line:" + CurrentLexem.RowNumber);
                }
            }
            else
            {
                return false;
            };
        }

        private bool IsSpysId()
        {

            if (TryNextLexem().LexemCode == 50 && TryNextLexem().type != 4)
            {
                GetNextLexem();
                if (TryNextLexem().LexemCode == 32)
                {
                    GetNextLexem();
                    if (!IsSpysId())
                    {
                        throw new Exceptions.MyException("Error! Variable expected Line:" + CurrentLexem.RowNumber);
                    }
                }
                else
                {
                    return true;
                };
            }

            else
            {
                throw new Exceptions.MyException("Error! Variable expected Line:" + CurrentLexem.RowNumber);
            }
            return true;
        }

    }
}
