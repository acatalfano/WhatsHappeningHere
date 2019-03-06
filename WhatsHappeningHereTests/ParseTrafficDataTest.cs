using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WhatsHappeningHere.HttpResources.DataObjects.ResponseObjects;
using WhatsHappeningHere.HttpResources.DataObjects.DataTransformations;
using WhatsHappeningHere.HttpResources.DataObjects.GeoJsonRequestObjects;

namespace WhatsHappeningHere.Test
{
    [TestClass]
    public class ParseTrafficDataTest
    {
        [TestMethod]
        public void Test_no_RWS()
        {
            List<TrafficParseData> expectedResult =
                new List<TrafficParseData>();

            List<TrafficParseData> actualResult = GetResultFromFile(@"C:\Users\Adam\source\repos\WhatsHappeningHere\WhatsHappeningHereTests\XML Test Files\no_rws.xml");

            try
            {
                MyAssert(expectedResult, actualResult);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }

        [TestMethod]
        public void Test_one_RWS_empty()
        {
            List<TrafficParseData> expectedResult =
                new List<TrafficParseData>();

            List<TrafficParseData> actualResult =
                GetResultFromFile(@"C:\Users\Adam\source\repos\WhatsHappeningHere\WhatsHappeningHereTests\XML Test Files\one_rws_empty.xml");

            try
            {
                MyAssert(expectedResult, actualResult);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }


        [TestMethod]
        public void Test_two_RWS_empty()
        {
            List<TrafficParseData> expectedResult =
                new List<TrafficParseData>();

            List<TrafficParseData> actualResult =
                GetResultFromFile(@"C:\Users\Adam\source\repos\WhatsHappeningHere\WhatsHappeningHereTests\XML Test Files\two_rws_empty.xml");

            try
            {
                MyAssert(expectedResult, actualResult);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }



        [TestMethod]
        public void Test_multiple_optionals_omitted()
        {
            List<TrafficParseData> expectedResult =
                new List<TrafficParseData>()
                {
                    new TrafficParseData()
                    {
                        RoadName = "Detroit-Windsor Tunl",
                        LinearIdentifier = "C09-00785",
                        FIList = new List<RoadwayData>()
                        {
                            new RoadwayData
                            {
                                PointTMCLocationCode = 10570,
                                RoadSegmentName = "US/Canada Border",
                                QueuingDirection = "+",
                                Length = 0.60936,
                                JamFactor = 3.21038,
                                Confidence = 0.7
                            }
                        }
                    },
                    new TrafficParseData()
                    {
                        FIList = new List<RoadwayData>()
                        {
                            new RoadwayData
                            {
                                JamFactor = 8.17712,
                                Confidence = 0.7,
                                Shape = new List<double[]>()
                                {
                                    new double[] { -83.03969, 42.32417 },
                                    new double[] { -83.03948, 42.32294 },
                                    new double[] { -83.03917, 42.32142 },
                                    new double[] { -83.03905, 42.32073 }
                                }
                            }
                        }
                    }
                };

            List<TrafficParseData> actualResult =
                GetResultFromFile(@"C:\Users\Adam\source\repos\WhatsHappeningHere\WhatsHappeningHereTests\XML Test Files\multiple_optionals_omitted.xml");

            try
            {
                MyAssert(expectedResult, actualResult);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }


        [TestMethod]
        public void Test_multiple_optionals_omitted_first_half()
        {
            List<TrafficParseData> expectedResult =
                new List<TrafficParseData>()
                {
                    new TrafficParseData()
                    {
                        RoadName = "Detroit-Windsor Tunl",
                        LinearIdentifier = "C09-00785",
                        FIList = new List<RoadwayData>()
                        {
                            new RoadwayData
                            {
                                PointTMCLocationCode = 10570,
                                RoadSegmentName = "US/Canada Border",
                                QueuingDirection = "+",
                                Length = 0.60936,
                                JamFactor = 3.21038,
                                Confidence = 0.7
                            }
                        }
                    }
                };

            List<TrafficParseData> actualResult =
                GetResultFromFile(@"C:\Users\Adam\source\repos\WhatsHappeningHere\WhatsHappeningHereTests\XML Test Files\multiple_optionals_omitted_first_half.xml");

            try
            {
                MyAssert(expectedResult, actualResult);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }


        [TestMethod]
        public void Test_multiple_optionals_omitted_second_half()
        {
            List<TrafficParseData> expectedResult =
                new List<TrafficParseData>()
                {
                    new TrafficParseData()
                    {
                        FIList = new List<RoadwayData>()
                        {
                            new RoadwayData
                            {
                                JamFactor = 8.17712,
                                Confidence = 0.7,
                                Shape = new List<double[]>()
                                {
                                    new double[] { -83.03969, 42.32417 },
                                    new double[] { -83.03948, 42.32294 },
                                    new double[] { -83.03917, 42.32142 },
                                    new double[] { -83.03905, 42.32073 }
                                }
                            }
                        }
                    }
                };

            List<TrafficParseData> actualResult =
                GetResultFromFile(@"C:\Users\Adam\source\repos\WhatsHappeningHere\WhatsHappeningHereTests\XML Test Files\multiple_optionals_omitted_second_half.xml");

            try
            {
                MyAssert(expectedResult, actualResult);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }



        [TestMethod]
        public void Test_minimal_tmc()
        {
            List<TrafficParseData> expectedResult =
                new List<TrafficParseData>()
                {
                    new TrafficParseData()
                    {
                        FIList = new List<RoadwayData>()
                        {
                            new RoadwayData
                            {
                                RoadSegmentName = "US/Canada Border",
                                QueuingDirection = "+",
                                Length = 0.60936,
                                JamFactor = 8.17712,
                                Confidence = 0.7,
                                Shape = new List<double[]>()
                                {
                                    new double[] { -83.03969, 42.32417 },
                                    new double[] { -83.03948, 42.32294 },
                                    new double[] { -83.03917, 42.32142 },
                                    new double[] { -83.03905, 42.32073 }
                                }
                            }
                        }
                    }
                };

            List<TrafficParseData> actualResult =
                GetResultFromFile(@"C:\Users\Adam\source\repos\WhatsHappeningHere\WhatsHappeningHereTests\XML Test Files\minimal_tmc.xml");

            try
            {
                MyAssert(expectedResult, actualResult);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }


        [TestMethod]
        public void Test_full_tmc()
        {
            List<TrafficParseData> expectedResult =
                new List<TrafficParseData>()
                {
                    new TrafficParseData()
                    {
                        FIList = new List<RoadwayData>()
                        {
                            new RoadwayData
                            {
                                PointTMCLocationCode = 10570,
                                RoadSegmentName = "US/Canada Border",
                                QueuingDirection = "+",
                                Length = 0.60936,
                                JamFactor = 8.17712,
                                Confidence = 0.7,
                                Shape = new List<double[]>()
                                {
                                    new double[] { -83.03969, 42.32417 },
                                    new double[] { -83.03948, 42.32294 },
                                    new double[] { -83.03917, 42.32142 },
                                    new double[] { -83.03905, 42.32073 }
                                }
                            }
                        }
                    }
                };

            List<TrafficParseData> actualResult =
                GetResultFromFile(@"C:\Users\Adam\source\repos\WhatsHappeningHere\WhatsHappeningHereTests\XML Test Files\full_tmc.xml");

            try
            {
                MyAssert(expectedResult, actualResult);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }


        [TestMethod]
        public void Test_full_tmc_two_shp()
        {
            List<TrafficParseData> expectedResult =
                new List<TrafficParseData>()
                {
                    new TrafficParseData()
                    {
                        FIList = new List<RoadwayData>()
                        {
                            new RoadwayData
                            {
                                PointTMCLocationCode = 10570,
                                RoadSegmentName = "US/Canada Border",
                                QueuingDirection = "+",
                                Length = 0.60936,
                                JamFactor = 8.17712,
                                Confidence = 0.7,
                                Shape = new List<double[]>()
                                {
                                    new double[] { -83.03969, 42.32417 },
                                    new double[] { -83.03948, 42.32294 },
                                    new double[] { -83.03917, 42.32142 },
                                    new double[] { -83.03905, 42.32073 },
                                    new double[] { -83.03912, 42.32066 },
                                    new double[] { -83.04001, 42.32102 }
                                }
                            }
                        }
                    }
                };

            List<TrafficParseData> actualResult =
                GetResultFromFile(@"C:\Users\Adam\source\repos\WhatsHappeningHere\WhatsHappeningHereTests\XML Test Files\full_tmc_two_shp.xml");

            try
            {
                MyAssert(expectedResult, actualResult);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }


        [TestMethod]
        public void Test_full_tmc_two_shp_add_rw_attributes()
        {
            List<TrafficParseData> expectedResult =
                new List<TrafficParseData>()
                {
                    new TrafficParseData()
                    {
                        RoadName = "Detroit-Windsor Tunl",
                        LinearIdentifier = "C09-00785",
                        FIList = new List<RoadwayData>()
                        {
                            new RoadwayData
                            {
                                PointTMCLocationCode = 10570,
                                RoadSegmentName = "US/Canada Border",
                                QueuingDirection = "+",
                                Length = 0.60936,
                                JamFactor = 8.17712,
                                Confidence = 0.7,
                                Shape = new List<double[]>()
                                {
                                    new double[] { -83.03969, 42.32417 },
                                    new double[] { -83.03948, 42.32294 },
                                    new double[] { -83.03917, 42.32142 },
                                    new double[] { -83.03905, 42.32073 },
                                    new double[] { -83.03912, 42.32066 },
                                    new double[] { -83.04001, 42.32102 }
                                }
                            }
                        }
                    }
                };

            List<TrafficParseData> actualResult =
                GetResultFromFile(@"C:\Users\Adam\source\repos\WhatsHappeningHere\WhatsHappeningHereTests\XML Test Files\full_tmc_two_shp_add_rw_attributes.xml");

            try
            {
                MyAssert(expectedResult, actualResult);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }


        [TestMethod]
        public void Test_rw_without_unparsed_attributes()
        {
            List<TrafficParseData> expectedResult =
                new List<TrafficParseData>()
                {
                    new TrafficParseData()
                    {
                        RoadName = "Detroit-Windsor Tunl",
                        LinearIdentifier = "C09-00785",
                        FIList = new List<RoadwayData>()
                        {
                            new RoadwayData
                            {
                                PointTMCLocationCode = 10570,
                                RoadSegmentName = "US/Canada Border",
                                QueuingDirection = "+",
                                Length = 0.60936,
                                JamFactor = 8.17712,
                                Confidence = 0.7,
                                Shape = new List<double[]>()
                                {
                                    new double[] { -83.03969, 42.32417 },
                                    new double[] { -83.03948, 42.32294 },
                                    new double[] { -83.03917, 42.32142 },
                                    new double[] { -83.03905, 42.32073 },
                                    new double[] { -83.03912, 42.32066 },
                                    new double[] { -83.04001, 42.32102 }
                                }
                            }
                        }
                    }
                };

            List<TrafficParseData> actualResult =
                GetResultFromFile(@"C:\Users\Adam\source\repos\WhatsHappeningHere\WhatsHappeningHereTests\XML Test Files\rw_without_unparsed_attributes.xml");

            try
            {
                MyAssert(expectedResult, actualResult);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }



        [TestMethod]
        public void Test_rw_without_li()
        {
            List<TrafficParseData> expectedResult =
                new List<TrafficParseData>()
                {
                    new TrafficParseData()
                    {
                        RoadName = "Detroit-Windsor Tunl",
                        FIList = new List<RoadwayData>()
                        {
                            new RoadwayData
                            {
                                PointTMCLocationCode = 10570,
                                RoadSegmentName = "US/Canada Border",
                                QueuingDirection = "+",
                                Length = 0.60936,
                                JamFactor = 8.17712,
                                Confidence = 0.7,
                                Shape = new List<double[]>()
                                {
                                    new double[] { -83.03969, 42.32417 },
                                    new double[] { -83.03948, 42.32294 },
                                    new double[] { -83.03917, 42.32142 },
                                    new double[] { -83.03905, 42.32073 },
                                    new double[] { -83.03912, 42.32066 },
                                    new double[] { -83.04001, 42.32102 }
                                }
                            }
                        }
                    }
                };

            List<TrafficParseData> actualResult =
                GetResultFromFile(@"C:\Users\Adam\source\repos\WhatsHappeningHere\WhatsHappeningHereTests\XML Test Files\rw_without_li.xml");

            try
            {
                MyAssert(expectedResult, actualResult);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }



        [TestMethod]
        public void Test_rw_without_de()
        {
            List<TrafficParseData> expectedResult =
                new List<TrafficParseData>()
                {
                    new TrafficParseData()
                    {
                        LinearIdentifier = "C09-00785",
                        FIList = new List<RoadwayData>()
                        {
                            new RoadwayData
                            {
                                PointTMCLocationCode = 10570,
                                RoadSegmentName = "US/Canada Border",
                                QueuingDirection = "+",
                                Length = 0.60936,
                                JamFactor = 8.17712,
                                Confidence = 0.7,
                                Shape = new List<double[]>()
                                {
                                    new double[] { -83.03969, 42.32417 },
                                    new double[] { -83.03948, 42.32294 },
                                    new double[] { -83.03917, 42.32142 },
                                    new double[] { -83.03905, 42.32073 },
                                    new double[] { -83.03912, 42.32066 },
                                    new double[] { -83.04001, 42.32102 }
                                }
                            }
                        }
                    }
                };

            List<TrafficParseData> actualResult =
                GetResultFromFile(@"C:\Users\Adam\source\repos\WhatsHappeningHere\WhatsHappeningHereTests\XML Test Files\rw_without_de.xml");

            try
            {
                MyAssert(expectedResult, actualResult);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }



        [TestMethod]
        public void Test_two_RWS_multiple_rw_each_multiple_fi_multiple_shp_and_tmc()
        {
            List<TrafficParseData> expectedResult =
                new List<TrafficParseData>()
                {
                    new TrafficParseData()
                    {
                        RoadName = "M-10",
                        LinearIdentifier = "108+00072",
                        FIList = new List<RoadwayData>()
                        {
                            new RoadwayData
                            {
                                PointTMCLocationCode = 9891,
                                RoadSegmentName = "M-1/Woodward Ave",
                                QueuingDirection = "-",
                                Length = 0.04528,
                                JamFactor = 2.9,
                                Confidence = 0.7,
                                Shape = new List<double[]>()
                                {
                                    new double[] { -83.04478, 42.32875 },
                                    new double[] { -83.04509, 42.32856 },
                                    new double[] { -83.04509, 42.32856 },
                                    new double[] { -83.04517, 42.32851 },
                                    new double[] { -83.04548, 42.32835 }
                                }
                            },
                            new RoadwayData
                            {
                                PointTMCLocationCode = 4288,
                                RoadSegmentName = "Jefferson Ave",
                                QueuingDirection = "-",
                                Length = 0.45126,
                                JamFactor = 0.0,
                                Confidence = 0.77,
                                Shape = new List<double[]>()
                                {
                                    new double[] { -83.04548, 42.32835},
                                    new double[] { -83.04558, 42.32831 },
                                    new double[] { -83.0511, 42.3264 },
                                    new double[] { -83.05149, 42.32641 },
                                    new double[] { -83.05149, 42.32641 },
                                    new double[] { -83.05155, 42.32641 },
                                    new double[] { -83.0517, 42.32642 }
                                }
                            },
                            new RoadwayData
                            {
                                PointTMCLocationCode = 4289,
                                RoadSegmentName = "Howard St",
                                QueuingDirection = "-",
                                Length = 0.47184,
                                JamFactor = 0.0,
                                Confidence = 0.84,
                                Shape = new List<double[]>()
                                {
                                    new double[] { -83.05762, 42.32857 },
                                    new double[] { -83.0578, 42.32871 },
                                    new double[] { -83.0578, 42.32871 },
                                    new double[] { -83.05803, 42.32888 },
                                    new double[] { -83.05835, 42.32913 },
                                    new double[] { -83.05879, 42.32947 },
                                    new double[] { -83.0593, 42.32989 },
                                    new double[] { -83.0593, 42.32989 },
                                    new double[] { -83.05954, 42.33009 },
                                    new double[] { -83.05961, 42.33015 }
                                }
                            }
                        }
                    },
                    new TrafficParseData()
                    {
                        RoadName = "M-10",
                        LinearIdentifier = "108-00072",
                        FIList = new List<RoadwayData>()
                        {
                            new RoadwayData
                            {
                                PointTMCLocationCode = 4288,
                                RoadSegmentName = "Jefferson Ave",
                                QueuingDirection = "+",
                                Length = 0.39658,
                                JamFactor = 0.0,
                                Confidence = 0.75,
                                Shape = new List<double[]>()
                                {
                                    new double[] {-83.05481, 42.32681},
                                    new double[] {-83.0547, 42.32677 },
                                    new double[] {-83.0547, 42.32677},
                                    new double[] {-83.05435, 42.32664}
                                }
                            },
                            new RoadwayData
                            {
                                PointTMCLocationCode = 9891,
                                RoadSegmentName = "M-1/Woodward Ave",
                                QueuingDirection = "+",
                                Length = 0.36784,
                                JamFactor = 0.0,
                                Confidence = 0.82,
                                Shape = new List<double[]>()
                                {
                                    new double[] { -83.04535, 42.3282 },
                                    new double[] { -83.04497, 42.32832 },
                                    new double[] { -83.04497, 42.32832 },
                                    new double[] { -83.04457, 42.32847 }
                                }
                            }
                        }
                    },
                    new TrafficParseData()
                    {
                        RoadName = "Detroit-Windsor Tunl",
                        LinearIdentifier = "C09-00785",
                        FIList = new List<RoadwayData>()
                        {
                            new RoadwayData
                            {
                                PointTMCLocationCode = 10570,
                                RoadSegmentName = "US/Canada Border",
                                QueuingDirection = "+",
                                Length = 0.60936,
                                JamFactor = 3.21038,
                                Confidence = 0.7
                            }
                        }
                    },
                    new TrafficParseData()
                    {
                        FIList = new List<RoadwayData>()
                        {
                            new RoadwayData
                            {
                                JamFactor = 8.17712,
                                Confidence = 0.7,
                                Shape = new List<double[]>()
                                {
                                    new double[] { -83.03969, 42.32417 },
                                    new double[] { -83.03948, 42.32294 },
                                    new double[] { -83.03917, 42.32142 },
                                    new double[] { -83.03905, 42.32073 }
                                }
                            }
                        }
                    }
                };

            List<TrafficParseData> actualResult =
                GetResultFromFile(@"C:\Users\Adam\source\repos\WhatsHappeningHere\WhatsHappeningHereTests\XML Test Files\two_rws_multiple_rw_each_multiple_fi_multiple_shp_and_tmc.xml");

            try
            {
                MyAssert(expectedResult, actualResult);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }



        [TestMethod]
        public void Test_To_Geojson_two_rws_multiple()
        {
            var expectedResultObject = new
            {
                Type = "FeatureCollection",
                Features = new List<Feature>
                {
                    new Feature
                    {
                        Properties = new PropertyData
                        {
                            Confidence = 0.7,
                            JamFactor = 2.9,
                            PointTMCLocationCode = 9891,
                            Length = 0.04528,
                            RoadSegmentName = "M-1/Woodward Ave",
                            QueuingDirection = "-",
                            RoadName = "M-10",
                            LinearIdentifier = "108+00072"
                        },
                        Geometry = new GeoData
                        {
                            Coordinates = new List<double[]>
                            {
                                new double[] { -83.04478, 42.32875 },
                                new double[] { -83.04509, 42.32856 },
                                new double[] { -83.04509, 42.32856 },
                                new double[] { -83.04517, 42.32851 },
                                new double[] { -83.04548, 42.32835 }
                            }
                        }
                    },
                    new Feature
                    {
                        Properties = new PropertyData
                        {
                            Confidence = 0.77,
                            JamFactor = 0.0,
                            PointTMCLocationCode = 4288,
                            Length = 0.45126,
                            RoadSegmentName = "Jefferson Ave",
                            QueuingDirection = "-",
                            RoadName = "M-10",
                            LinearIdentifier = "108+00072"
                        },
                        Geometry = new GeoData
                        {
                            Coordinates = new List<double[]>
                            {
                                new double[] { -83.04548, 42.32835},
                                new double[] { -83.04558, 42.32831 },
                                new double[] { -83.0511, 42.3264 },
                                new double[] { -83.05149, 42.32641 },
                                new double[] { -83.05149, 42.32641 },
                                new double[] { -83.05155, 42.32641 },
                                new double[] { -83.0517, 42.32642 }
                            }
                        }
                    },
                    new Feature
                    {
                        Properties = new PropertyData
                        {
                            Confidence = 0.84,
                            JamFactor = 0.0,
                            PointTMCLocationCode = 4289,
                            Length = 0.47184,
                            RoadSegmentName = "Howard St",
                            QueuingDirection = "-",
                            RoadName = "M-10",
                            LinearIdentifier = "108+00072"
                        },
                        Geometry = new GeoData
                        {
                            Coordinates = new List<double[]>
                            {
                                new double[] { -83.05762, 42.32857 },
                                new double[] { -83.0578, 42.32871 },
                                new double[] { -83.0578, 42.32871 },
                                new double[] { -83.05803, 42.32888 },
                                new double[] { -83.05835, 42.32913 },
                                new double[] { -83.05879, 42.32947 },
                                new double[] { -83.0593, 42.32989 },
                                new double[] { -83.0593, 42.32989 },
                                new double[] { -83.05954, 42.33009 },
                                new double[] { -83.05961, 42.33015 }
                            }
                        }
                    },
                    new Feature
                    {
                        Properties = new PropertyData
                        {
                            Confidence = 0.75,
                            JamFactor = 0.0,
                            PointTMCLocationCode = 4288,
                            Length = 0.39658,
                            RoadSegmentName = "Jefferson Ave",
                            QueuingDirection = "+",
                            RoadName = "M-10",
                            LinearIdentifier = "108-00072"
                        },
                        Geometry = new GeoData
                        {
                            Coordinates = new List<double[]>
                            {
                                new double[] {-83.05481, 42.32681},
                                new double[] {-83.0547, 42.32677 },
                                new double[] {-83.0547, 42.32677},
                                new double[] {-83.05435, 42.32664}
                            }
                        }
                    },
                    new Feature
                    {
                        Properties = new PropertyData
                        {
                            Confidence = 0.82,
                            JamFactor = 0.0,
                            PointTMCLocationCode = 9891,
                            Length = 0.36784,
                            RoadSegmentName = "M-1/Woodward Ave",
                            QueuingDirection = "+",
                            RoadName = "M-10",
                            LinearIdentifier = "108-00072"
                        },
                        Geometry = new GeoData
                        {
                            Coordinates = new List<double[]>
                            {
                                new double[] { -83.04535, 42.3282 },
                                new double[] { -83.04497, 42.32832 },
                                new double[] { -83.04497, 42.32832 },
                                new double[] { -83.04457, 42.32847 }
                            }
                        }
                    },
                    new Feature
                    {
                        Properties = new PropertyData
                        {
                            Confidence = 0.7,
                            JamFactor = 3.21038,
                            PointTMCLocationCode = 10570,
                            Length = 0.60936,
                            RoadSegmentName = "US/Canada Border",
                            QueuingDirection = "+",
                            RoadName = "Detroit-Windsor Tunl",
                            LinearIdentifier = "C09-00785"
                        },
                        Geometry = new GeoData
                        {
                            Coordinates = new List<double[]>()
                        }
                    },
                    new Feature
                    {
                        Properties = new PropertyData
                        {
                            Confidence = 0.7,
                            JamFactor = 8.17712,
                        },
                        Geometry = new GeoData
                        {
                            Coordinates = new List<double[]>
                            {
                                new double[] { -83.03969, 42.32417 },
                                new double[] { -83.03948, 42.32294 },
                                new double[] { -83.03917, 42.32142 },
                                new double[] { -83.03905, 42.32073 }
                            }
                        }
                    },
                }
            };

            string expectedResult = JsonConvert.SerializeObject(expectedResultObject, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            List<TrafficParseData> listResult =
                GetResultFromFile(@"C:\Users\Adam\source\repos\WhatsHappeningHere\WhatsHappeningHereTests\XML Test Files\two_rws_multiple_rw_each_multiple_fi_multiple_shp_and_tmc.xml");
            string actualResult = HereTrafficToMapboxLayer.GenerateGeoJSON(listResult);

            Assert.AreEqual(expectedResult, actualResult, $"The GeoJSON strings are different (expected, then actual):\n{expectedResult}\n{actualResult}");
        }



        private void MyAssert(List<TrafficParseData> expected, List<TrafficParseData> actual)
        {
            Assert.AreEqual(expected.Count, actual.Count,
                $"Different size of TrafficParseData.\n" +
                    $"\texpected: {expected.Count}\n" +
                    $"\tactual: {actual.Count}");
            
            var listPair = expected.Zip(actual, (e, a) => new { exp = e, act = a });
            int listPairIndex = 0;
            foreach (var item in listPair)
            {
                Assert.AreEqual(item.exp.LinearIdentifier, item.act.LinearIdentifier,
                    $"Different LinearIdentifier at index [{listPairIndex}].\n" + 
                        $"\texpected: {item.exp.LinearIdentifier}\n" + 
                        $"\tactual: {item.act.LinearIdentifier}");
                Assert.AreEqual(item.exp.RoadName, item.act.RoadName,
                    $"Different RoadName at index [{listPairIndex}].\n" +
                        $"\texpected: {item.exp.RoadName}\n" +
                        $"\tactual: {item.act.RoadName}");

                Assert.AreEqual(item.exp.FIList.Count, item.act.FIList.Count,
                    $"Different size of FIList at index [{listPairIndex}].\n" +
                        $"\texpected: {item.exp.FIList.Count}\n" +
                        $"\tactual: {item.act.FIList.Count}");
                var FIListPair = item.exp.FIList.Zip(item.act.FIList, (e, a) => new { exp = e, act = a });
                int FIListPairIndex = 0;
                foreach (var el in FIListPair)
                {
                    Assert.AreEqual(el.exp.PointTMCLocationCode, el.act.PointTMCLocationCode,
                        $"Different PointTMCLocationCode at index [{listPairIndex}, {FIListPairIndex}].\n" +
                            $"\texpected: {el.exp.PointTMCLocationCode}\n" +
                            $"\tactual: {el.act.PointTMCLocationCode}");
                    Assert.AreEqual(el.exp.RoadSegmentName, el.act.RoadSegmentName,
                        $"Different RoadSegmentName at index [{listPairIndex}, {FIListPairIndex}].\n" +
                            $"\texpected: {el.exp.RoadSegmentName}\n" +
                            $"\tactual: {el.act.RoadSegmentName}");
                    Assert.AreEqual(el.exp.QueuingDirection, el.act.QueuingDirection,
                        $"Different QueuingDirection at index [{listPairIndex}, {FIListPairIndex}].\n" +
                            $"\texpected: {el.exp.QueuingDirection}\n" +
                            $"\tactual: {el.act.QueuingDirection}");
                    Assert.AreEqual(el.exp.Length, el.act.Length,
                        $"Different Length value at index [{listPairIndex}, {FIListPairIndex}].\n" +
                            $"\texpected: {el.exp.Length}\n" +
                            $"\tactual: {el.act.Length}");
                    Assert.AreEqual(el.exp.JamFactor, el.act.JamFactor,
                        $"Different JamFactor at index [{listPairIndex}, {FIListPairIndex}].\n" +
                            $"\texpected: {el.exp.JamFactor}\n" +
                            $"\tactual: {el.act.JamFactor}");
                    Assert.AreEqual(el.exp.Confidence, el.act.Confidence,
                        $"Different Confidence at index [{listPairIndex}, {FIListPairIndex}].\n" +
                            $"\texpected: {el.exp.Confidence}\n" +
                            $"\tactual: {el.act.Confidence}");

                    bool expression1 = el.exp.Shape == null;
                    bool expression2 = el.act.Shape != null;
                    bool compound = expression1 && expression2;
                    
                    Assert.IsTrue(!(el.exp.Shape == null && el.act.Shape != null), $"Expected shape is null and actual shape is not null at index [{listPairIndex}, {FIListPairIndex}]");
                    Assert.IsTrue(!(el.exp.Shape != null && el.act.Shape == null), $"Actual shape is null and expected shape is not null at index [{listPairIndex}, {FIListPairIndex}]");
                    Assert.IsTrue(!(el.exp.Shape == null && el.act.Shape == null), $"Both actual shape and expected shape are null at index [{listPairIndex}, {FIListPairIndex}]");
                    Assert.AreNotEqual(el.exp.Shape, null, $"Expected shape is null at index [{listPairIndex}, {FIListPairIndex}]");
                    Assert.AreNotEqual(el.act.Shape, null, $"Actual shape is null at index [{listPairIndex}, {FIListPairIndex}]");
                    
                    Assert.AreEqual(el.exp.Shape.Count, el.act.Shape.Count,
                        $"Different size of Shape at index [{listPairIndex}, {FIListPairIndex}].\n" +
                            $"\texpected: {el.exp.Shape.Count}\n" +
                            $"\tactual: {el.act.Shape.Count}");
                    var shapePair = el.exp.Shape.Zip(el.act.Shape, (e, a) => new { exp = e, act = a });
                    int shapePairIndex = 0;
                    foreach (var coord in shapePair)
                    {
                        Assert.AreNotEqual(coord, null, $"Coordinate pair at index [{listPairIndex}, {FIListPairIndex}, {shapePairIndex}] is null.");
                        Assert.AreNotEqual(coord.exp, null, $"Expected coordinate pair at index [{listPairIndex}, {FIListPairIndex}, {shapePairIndex}] is null.");
                        Assert.AreNotEqual(coord.act, null, $"Actual coordinate pair at index [{listPairIndex}, {FIListPairIndex}, {shapePairIndex}] is null.");

                        Assert.AreEqual(coord.exp.Length, coord.act.Length,
                            $"Different coordinate pair length at index [{listPairIndex}, {FIListPairIndex}, {shapePairIndex}].\n" +
                                $"\texpected: {coord.exp.Length}\n" +
                                $"\tactual: {coord.act.Length}");
                        Assert.AreEqual(coord.exp.Length, 2,
                            $"Coordinate pair length is not 2 at index [{listPairIndex}, {FIListPairIndex}, {shapePairIndex}].\n" +
                                $"\texpected: {coord.exp.Length}\n" +
                                $"\tactual: {coord.act.Length}");
                        Assert.AreEqual(coord.exp[0], coord.act[0],
                            $"Different first coordinate at index [{listPairIndex}, {FIListPairIndex}, {shapePairIndex}].\n" +
                                $"\texpected: {coord.exp[0]}\n" +
                                $"\tactual: {coord.act[0]}");
                        Assert.AreEqual(coord.exp[1], coord.act[1],
                            $"Different second coordinate at index [{listPairIndex}, {FIListPairIndex}, {shapePairIndex}].\n" +
                                $"\texpected: {coord.exp[1]}\n" +
                                $"\tactual: {coord.act[1]}");

                        shapePairIndex++;
                    }

                    FIListPairIndex++;
                }

                listPairIndex++;
            }
        }



        private List<TrafficParseData> GetResultFromFile(string filename)
        {
            string xmlContent = File.ReadAllText(filename);
            List<TrafficParseData> response = HereTrafficToMapboxLayer.ParseTrafficData(xmlContent);

            return response;
        }
    }
}
