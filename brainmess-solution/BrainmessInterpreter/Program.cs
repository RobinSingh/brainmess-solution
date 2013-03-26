using System;
using System.Collections.Generic;
using System.IO;

namespace BrainmessInterpreter
{
    public class Program
    {

        //private const int MemoryLength = 32767;

        static string _inputInstruction;
        public string output = "";
        public short[] tapememory;
        public int ptrMemory;
        public int ptrInstruction;
        static Stack<int> _loopStack;
        private static Dictionary<int, int> _jmpTable;
        public short readInput;
        
        static void Main()
        {
            if (Constant.FilePath.Length == 0)
            {
                Console.WriteLine("Provide path to the source file.");
                Console.ReadLine();
                return;
            }


            var sourceFile = new FileInfo(Constant.FilePath);
            if (!sourceFile.Exists)
            {
                Console.WriteLine("File not found.");
                return;
            }

            var objProgram = new Program();
            objProgram.BrainMessCompiler(sourceFile);
            Console.Read();

        }

        public void BrainMessCompiler(FileInfo sourceFile)
        {
            try
            {
                using (StreamReader reader = sourceFile.OpenText())
                {
                    _inputInstruction = reader.ReadToEnd();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to read the input file.");
                return;
            }

            tapememory = new short[Constant.MemoryLength];
            ptrInstruction = 0;
            ptrMemory = 0;
            _loopStack = new Stack<int>();
            _jmpTable = new Dictionary<int, int>();

            // start building the jump table. The jump table is a key-value store which matches positions of brackets [ and ]
            
            for (int i = 0; i < _inputInstruction.Length; i++)
            {
                switch (_inputInstruction[i])
                {
                    case '[':
                        _loopStack.Push(i);
                        break;
                    case ']':
                        if (_loopStack.Count == 0)
                        {
                            // if the stack is empty, then there is no matching counterpart for this ] bracket
                            Console.WriteLine("No matching ] bracket found.");
                            return;
                        }
                        else
                        {
                            // if the stack is non-empty, then the top element on the stack is the position of the matching [ bracket
                            var openPos = _loopStack.Pop();
                            _jmpTable.Add(openPos, i + 1);
                            _jmpTable.Add(i, openPos + 1);
                        }
                        break;
                }
            }

            if (_loopStack.Count > 0)
            {
                Console.WriteLine("There are one or more [ brackets with no matching counterparts.");
                return;
            }

            while (ptrInstruction < _inputInstruction.Length)
            {
                switch (_inputInstruction[ptrInstruction])
                {
                    case '>':
                        IncrementMemoryAddress();
                        break;
                    case '<':
                        DecrementMemoryAddress();
                        break;
                    case '+':
                        IncrementMemoryValue();
                        break;
                    case '-':
                        DecrementMemoryValue();
                        break;
                    case '.':
                        ShowOutput();
                        break;
                    case ',':
                        ReadInput();
                        break;
                    case '[':
                        StartLoop();
                        break;
                    case ']':
                        EndLoop();
                        break;
                    default:
                        ptrInstruction++;
                        break;
                }

            }
        }

        public void IncrementMemoryAddress()
        {
            ptrMemory++;
            if (ptrMemory == Constant.MemoryLength)
                ptrMemory = 0;
            ptrInstruction++;
        }

        public void DecrementMemoryAddress()
        {
            ptrMemory--;
            if (ptrMemory == -1)
                ptrMemory = Constant.MemoryLength - 1;
            ptrInstruction++;
        }
        public void IncrementMemoryValue()
        {
            tapememory[ptrMemory]++;
            ptrInstruction++;
        }

        public void DecrementMemoryValue()
        {
            tapememory[ptrMemory]--;
            ptrInstruction++;
        }

        public void ShowOutput()
        {
            Console.Write(Convert.ToChar(tapememory[ptrMemory]));
            output = output + Convert.ToChar(tapememory[ptrMemory]);
            ptrInstruction++;
        }

        public void ReadInput()
        {
            readInput = (short)Console.Read();
            tapememory[ptrMemory] = readInput;
            ptrInstruction++;
        }

        public void StartLoop()
        {
            if (tapememory[ptrMemory] == 0)
            {
                ptrInstruction = _jmpTable[ptrInstruction];
            }
            else
            {
                ptrInstruction++;
            }
        }

        public void EndLoop()
        {
            if (tapememory[ptrMemory] == 0)
            {
                ptrInstruction++;
            }
            else
            {
                ptrInstruction = _jmpTable[ptrInstruction];
            }
        }

    }
}
