using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace CardData.Helpers
{
    public class GenerateCardHelper
    {
        static Random random = new Random();
        public static int GenerateRandomInt(int minVal = 10, int maxVal = 99)
        {
            var rnd = new byte[4];
            using (var rng = new RNGCryptoServiceProvider())
                rng.GetBytes(rnd);
            var i = Math.Abs(BitConverter.ToInt32(rnd, 0));
            return Convert.ToInt32(i % (maxVal - minVal + 1) + minVal);
        }

        public static string GenerateVoucherCode(int length)
        {
            const string chars = "2983401567";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string GenerateVoucherCodeBatch()
        {
            const string chars = "1923405678";
            string card = string.Empty;
            for (int i = 1; i<=4; i++)
            {
              string code =  new string(Enumerable.Repeat(chars, 5)
              .Select(s => s[random.Next(s.Length)]).ToArray());

                card += code;
            }

            return card;
        }

        /**
                public static ReturnCard GenerateCards(string prefix, int batchstartnumber, int total, int cvv, int cardtype, int batchid, string reqby)
                {
                    List<CardInfo> list = new List<CardInfo>();
                    int expmnonth = 0;
                    int expyear = 0;
                    DateTime expTime = DateTime.Now.AddYears(2);

                    expmnonth = expTime.Month;
                    expyear = expTime.Year;

                    if (total > 1000)
                    {
                        total = 1000;
                    }
                    for (int i = 1; i <= total; i++)
                    {
                        string card = "";
                        int bc = GenerateRandomInt(1000, 9999);
                        int cata = GenerateRandomInt(10, 90);
                        int catb = GenerateRandomInt(30, 99);
                        string bc2 = "";//GenerateRandomInt(1000, 9999);

                        if (batchstartnumber < 1000)
                        {
                            if (batchstartnumber > 0 && batchstartnumber < 10)
                            {
                                bc2 = "000" + batchstartnumber.ToString();
                            }
                            else if (batchstartnumber >= 10 && batchstartnumber < 100)
                            {
                                bc2 = "00" + batchstartnumber.ToString();
                            }
                            else if (batchstartnumber >= 100 && batchstartnumber < 1000)
                            {
                                bc2 = "0" + batchstartnumber.ToString();
                            }
                        }
                        else
                        {
                            bc2 = batchstartnumber.ToString();
                        }
                        card = string.Format("{4}{0}{1}{2}{3}", bc, catb, cata, bc2, prefix, batchstartnumber);


                        list.Add(new CardInfo()
                        {
                            CardNumber = card,
                            CardType = cardtype,
                            CardKey = Guid.NewGuid().ToString(),
                            MonthExpire = expmnonth,
                            YearExpire = expyear,
                            CVV = cvv,
                            CardBatch = batchid,
                            Status = Constants.CardActive,
                            IsActivated = false,
                            DateCreated = DateTime.Now,
                            CreatedBy = reqby,
                            CardSerial = bc2

                        }
                        );

                        cvv++;
                        batchstartnumber++;
                    }

                    //list.Sort();

                    return new ReturnCard { CardInfos = list, LastNumber = batchstartnumber };
                }

                **/
    }
}
