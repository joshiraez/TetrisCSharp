using System.Collections.Generic;

namespace TetrisCSharp.GameStatus.TetrisPieces
{
    //0,0 is the pivot point (piece position in world). x positive to the right. y positive down
    //Could be improved by using Sega Rotation values (the values used by Arika ) so all pieces are flatten up, and changing pivot point.

    public static class TetrisPieceRotationData
    {
        private static Dictionary<TetrisPieceEnum, Position[][]> positionDiccionary = new Dictionary<TetrisPieceEnum, Position[][]> 
        {
            {
                TetrisPieceEnum.O,
                new Position[][]
                {
                    new Position[]
                    {
                        new Position(0,0),
                        new Position(0,1),
                        new Position(1,0),
                        new Position(1,1)
                    } 
                }
            },

            {
                TetrisPieceEnum.J,
                new Position[][]
                {
                    new Position[]
                    {
                        new Position(-1,0),
                        new Position(0,0),
                        new Position(0,1),
                        new Position(0,2)
                        
                    },
                    new Position[]
                    {
                        new Position(-2,0),
                        new Position(-1,0),
                        new Position(0,-1),
                        new Position(0,0)
                    },
                    new Position[]
                    {
                        new Position(0,-2),
                        new Position(0,-1),
                        new Position(0,0),
                        new Position(1,0)
                    },
                    new Position[]
                    {
                        new Position(0,0),
                        new Position(0,1),
                        new Position(1,0),
                        new Position(2,0)
                    }
                }
            },

            {
                TetrisPieceEnum.L,
                new Position[][]
                {
                    new Position[]
                    {
                        new Position(0,0),
                        new Position(0,1),
                        new Position(0,2),
                        new Position(1,0)

                    },
                    new Position[]
                    {
                        new Position(-2,0),
                        new Position(-1,0),
                        new Position(0,0),
                        new Position(0,1)
                    },
                    new Position[]
                    {
                        new Position(-1,0),
                        new Position(0,-2),
                        new Position(0,-1),
                        new Position(0,0)
                    },
                    new Position[]
                    {
                        new Position(0,-1),
                        new Position(0,0),
                        new Position(1,0),
                        new Position(2,0)
                    }
                }
            },

            {
                TetrisPieceEnum.T,
                new Position[][]
                {
                    new Position[]
                    {
                        new Position(0,-1),
                        new Position(0,0),
                        new Position(0,1),
                        new Position(1,0)
                    },
                    new Position[]
                    {
                        new Position(-1,0),
                        new Position(0,0),
                        new Position(1,0),
                        new Position(0,1)
                    },
                    new Position[]
                    {
                        new Position(-1,0),
                        new Position(0,-1),
                        new Position(0,0),
                        new Position(0,1)
                    },
                    new Position[]
                    {
                        new Position(-1,0),
                        new Position(0,-1),
                        new Position(0,0),
                        new Position(1,0)
                    }
                }
            },

             {
                TetrisPieceEnum.I,
                new Position[][]
                {
                    new Position[]
                    {
                        new Position(0,-1),
                        new Position(0,0),
                        new Position(0,1),
                        new Position(0,2)
                    },
                    new Position[]
                    {
                        new Position(-1,0),
                        new Position(0,0),
                        new Position(1,0),
                        new Position(2,0)
                    },
                }
            },

            {
                TetrisPieceEnum.S,
                new Position[][]
                {
                    new Position[]
                    {
                        new Position(-1,0),
                        new Position(-1,1),
                        new Position(0,-1),
                        new Position(0,0)
                    },
                    new Position[]
                    {
                        new Position(-1,-1),
                        new Position(0,-1),
                        new Position(0,0),
                        new Position(1,0)
                    }
                }

            },

            {
                TetrisPieceEnum.Z,
                new Position[][]
                {
                    new Position[]
                    {
                        new Position(-1,-1),
                        new Position(-1,0),
                        new Position(0,0),
                        new Position(0,1)
                    },
                    new Position[]
                    {
                        new Position(-1,1),
                        new Position(0,0),
                        new Position(0,1),
                        new Position(1,0)
                    }
                }

            }
        };
        
        public static Position[][] getRotations(this TetrisPieceEnum type)
        {
            Position[][] res = new Position[][] { new Position[] { } };

            if (positionDiccionary.ContainsKey(type))
            {
                res= positionDiccionary[type];
            }

            return res;
        }
    }
}
