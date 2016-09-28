using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapzenGo.Models.Enums;
using UnityEngine;

namespace MapzenGo.Helpers
{
    public static class Extensions
    {
        public static Dictionary<string, PlaceType> PlaceTypes = new Dictionary<string, PlaceType>()
        {
            {"unknown", PlaceType.Unknown},
            {"borough",PlaceType.Borough},
            {"city",PlaceType.City},
            {"continent",PlaceType.Continent},
            {"country",PlaceType.Country},
            {"farm",PlaceType.Farm},
            {"hamlet",PlaceType.Hamlet},
            {"historic_place",PlaceType.HistoricPlace},
            {"isolatedDwelling",PlaceType.IsolatedDwelling},
            {"locality",PlaceType.Locality},
            {"macrohood",PlaceType.Macrohood},
            {"meteorologicalStation",PlaceType.MeteorologicalStation},
            {"neighbourhood",PlaceType.Neighbourhood},
            {"populatedPlace",PlaceType.PopulatedPlace},
            {"province",PlaceType.Province},
            {"quarter",PlaceType.Quarter},
            {"scientificStation",PlaceType.ScientificStation},
            {"state",PlaceType.State},
            {"suburb",PlaceType.Suburb},
            {"town",PlaceType.Town},
            {"village",PlaceType.Village},
        };

        public static Dictionary<string, BoundaryType> BoundaryTypes = new Dictionary<string, BoundaryType>()
        {
            {"unknown", BoundaryType.Unknown},
            {"aboriginal_Lands", BoundaryType.Aboriginal_Lands},
            {"city_Wall", BoundaryType.City_Wall},
            {"country", BoundaryType.Country},
            {"county", BoundaryType.County},
            {"dam", BoundaryType.Dam},
            {"disputed", BoundaryType.Disputed},
            {"fence", BoundaryType.Fence},
            {"indefinite", BoundaryType.Indefinite},
            {"indeterminate", BoundaryType.Indeterminate},
            {"lease_Limit", BoundaryType.Lease_Limit},
            {"line_Of_Control", BoundaryType.Line_Of_Control},
            {"macroregion", BoundaryType.Macroregion},
            {"municipality", BoundaryType.Municipality},
            {"overlay_Limit", BoundaryType.Overlay_Limit},
            {"retaining_Wall", BoundaryType.Retaining_Wall},
            {"snow_Fence", BoundaryType.Snow_Fence},
            {"state", BoundaryType.State}
        };

        public static Dictionary<string, LanduseKind> LanduseKinds = new Dictionary<string, LanduseKind>()
        {
            {"unknown", LanduseKind.Unknown},
            {"aerodrome", LanduseKind.Aerodrome},
            {"allotments", LanduseKind.Allotments},
            {"amusement_ride", LanduseKind.Amusement_ride},
            {"animal", LanduseKind.Animal},
            {"apron", LanduseKind.Apron},
            {"aquarium", LanduseKind.Aquarium},
            {"artwork", LanduseKind.Artwork},
            {"attraction", LanduseKind.Attraction},
            {"aviary", LanduseKind.Aviary},
            {"battlefield", LanduseKind.Battlefield},
            {"beach", LanduseKind.Beach},
            {"breakwater", LanduseKind.Breakwater},
            {"bridge", LanduseKind.Bridge},
            {"caravan_site", LanduseKind.Caravan_site},
            {"carousel", LanduseKind.Carousel},
            {"cemetery", LanduseKind.Cemetery},
            {"cinema", LanduseKind.Cinema},
            {"college", LanduseKind.College},
            {"commercial", LanduseKind.Commercial},
            {"common", LanduseKind.Common},
            {"cutline", LanduseKind.Cutline},
            {"dam", LanduseKind.Dam},
            {"dike", LanduseKind.Dike},
            {"dog_park", LanduseKind.Dog_park},
            {"enclosure", LanduseKind.Enclosure},
            {"farm", LanduseKind.Farm},
            {"farmland", LanduseKind.Farmland},
            {"farmyard", LanduseKind.Farmyard},
            {"footway", LanduseKind.Footway},
            {"forest", LanduseKind.Forest},
            {"fort", LanduseKind.Fort},
            {"fuel", LanduseKind.Fuel},
            {"garden", LanduseKind.Garden},
            {"generator", LanduseKind.Generator},
            {"glacier", LanduseKind.Glacier},
            {"golf_course", LanduseKind.Golf_course},
            {"grass", LanduseKind.Grass},
            {"groyne", LanduseKind.Groyne},
            {"hanami", LanduseKind.Hanami},
            {"hospital", LanduseKind.Hospital},
            {"ındustrial", LanduseKind.Industrial},
            {"land", LanduseKind.Land},
            {"library", LanduseKind.Library},
            {"maze", LanduseKind.Maze},
            {"meadow", LanduseKind.Meadow},
            {"military", LanduseKind.Military},
            {"national_park", LanduseKind.National_park},
            {"nature_reserve", LanduseKind.Nature_reserve},
            {"park", LanduseKind.Park},
            {"protected_land", LanduseKind.Protected_land},
            {"parking", LanduseKind.Parking},
            {"pedestrian", LanduseKind.Pedestrian},
            {"petting_zoo", LanduseKind.Petting_zoo},
            {"picnic_site", LanduseKind.Picnic_site},
            {"pier", LanduseKind.Pier},
            {"pitch", LanduseKind.Pitch},
            {"place_of_worship", LanduseKind.Place_of_worship},
            {"plant", LanduseKind.Plant},
            {"playground", LanduseKind.Playground},
            {"prison", LanduseKind.Prison},
            {"protected_area", LanduseKind.Protected_area},
            {"quarry", LanduseKind.Quarry},
            {"railway", LanduseKind.Railway},
            {"recreation_ground", LanduseKind.Recreation_ground},
            {"recreation_track", LanduseKind.Recreation_track},
            {"residential", LanduseKind.Residential},
            {"resort", LanduseKind.Resort},
            {"retail", LanduseKind.Retail},
            {"rock", LanduseKind.Rock},
            {"roller_coaster", LanduseKind.Roller_coaster},
            {"runway", LanduseKind.Runway},
            {"rural", LanduseKind.Rural},
            {"school", LanduseKind.School},
            {"scree", LanduseKind.Scree},
            {"scrub", LanduseKind.Scrub},
            {"sports_centre", LanduseKind.Sports_centre},
            {"stadium", LanduseKind.Stadium},
            {"stone", LanduseKind.Stone},
            {"substation", LanduseKind.Substation},
            {"summer_toboggan", LanduseKind.Summer_toboggan},
            {"taxiway", LanduseKind.Taxiway},
            {"theatre", LanduseKind.Theatre},
            {"theme_park", LanduseKind.Theme_park},
            {"tower", LanduseKind.Tower},
            {"trail_riding_station", LanduseKind.Trail_riding_station},
            {"university", LanduseKind.University},
            {"urban_area", LanduseKind.Urban_area},
            {"urban", LanduseKind.Urban},
            {"village_green", LanduseKind.Village_green},
            {"wastewater_plant", LanduseKind.Wastewater_plant},
            {"water_park", LanduseKind.Water_park},
            {"water_slide", LanduseKind.Water_slide},
            {"water_works", LanduseKind.Water_works},
            {"wetland", LanduseKind.Wetland},
            {"wilderness_hut", LanduseKind.Wilderness_hut},
            {"wildlife_park", LanduseKind.Wildlife_park},
            {"winery", LanduseKind.Winery},
            {"winter_sports", LanduseKind.Winter_sports},
            {"wood", LanduseKind.Wood},
            {"works", LanduseKind.Works},
            {"zoo", LanduseKind.Zoo},
        };

        public static Dictionary<string, RailwayType> RailwayTypes = new Dictionary<string, RailwayType>()
        {
            {"unknown", RailwayType.Unknown},
            {"Construction", RailwayType.Construction},
            {"Disused", RailwayType.Disused},
            {"Funicular", RailwayType.Funicular},
            {"Light_rail", RailwayType.Light_rail},
            {"Miniature", RailwayType.Miniature},
            {"Monorail", RailwayType.Monorail},
            {"Narrow_gauge", RailwayType.Narrow_gauge},
            {"Preserved", RailwayType.Preserved},
            {"Rail", RailwayType.Rail},
            {"Subway", RailwayType.Subway},
            {"Tram", RailwayType.Tram},
        };

        public static Dictionary<string, WaterType> WaterTypes = new Dictionary<string, WaterType>()
        {
            {"unknown", WaterType.Unknown},
            {"basin", WaterType.Basin},
            {"dock", WaterType.Dock},
            {"lake", WaterType.Lake},
            {"ocean", WaterType.Ocean},
            {"playa", WaterType.Playa},
            {"riverbank", WaterType.Riverbank},
            {"swimming_Pool", WaterType.Swimming_Pool},
            {"water", WaterType.Water},
        };

        public static Dictionary<string, EarthType> EarthTypes = new Dictionary<string, EarthType>()
        {
            {"unknown", EarthType.Unknown},
            {"arete",  EarthType.Arete},
            {"cliff",  EarthType.Cliff},
            {"earth",  EarthType.Earth},
            {"ridge",  EarthType.Ridge},
            {"valley", EarthType.Valley},
        };

        public static Dictionary<string, RoadType> RoadTypes = new Dictionary<string, RoadType>()
        {
            {"unknown", RoadType.Unknown},
            {"aerialway"  , RoadType.Aerialway},
            {"exit"       , RoadType.Exit},
            {"ferry"      , RoadType.Ferry},
            {"highway"    , RoadType.Highway},
            {"major_road" , RoadType.Major_Road},
            {"minor_road" , RoadType.Minor_Road},
            {"path"       , RoadType.Path},
            {"piste"      , RoadType.Piste},
            {"racetrack"  , RoadType.Racetrack},
            {"rail"       , RoadType.Rail}
        };

        public static Dictionary<string, BuildingType> BuildingTypes = new Dictionary<string, BuildingType>()
        {
             {"unknown", BuildingType.Unknown},
            {"aerodrome", BuildingType.Aerodrome},
            {"allotments", BuildingType.Allotments},
            {"amusement_ride", BuildingType.Amusement_ride},
            {"animal", BuildingType.Animal},
            {"apron", BuildingType.Apron},
            {"aquarium", BuildingType.Aquarium},
            {"artwork", BuildingType.Artwork},
            {"attraction", BuildingType.Attraction},
            {"aviary", BuildingType.Aviary},
            {"battlefield", BuildingType.Battlefield},
            {"beach", BuildingType.Beach},
            {"breakwater", BuildingType.Breakwater},
            {"bridge", BuildingType.Bridge},
            {"caravan_site", BuildingType.Caravan_site},
            {"carousel", BuildingType.Carousel},
            {"cemetery", BuildingType.Cemetery},
            {"cinema", BuildingType.Cinema},
            {"college", BuildingType.College},
            {"commercial", BuildingType.Commercial},
            {"common", BuildingType.Common},
            {"cutline", BuildingType.Cutline},
            {"dam", BuildingType.Dam},
            {"dike", BuildingType.Dike},
            {"dog_park", BuildingType.Dog_park},
            {"enclosure", BuildingType.Enclosure},
            {"farm", BuildingType.Farm},
            {"farmland", BuildingType.Farmland},
            {"farmyard", BuildingType.Farmyard},
            {"footway", BuildingType.Footway},
            {"forest", BuildingType.Forest},
            {"fort", BuildingType.Fort},
            {"fuel", BuildingType.Fuel},
            {"garden", BuildingType.Garden},
            {"generator", BuildingType.Generator},
            {"glacier", BuildingType.Glacier},
            {"golf_course", BuildingType.Golf_course},
            {"grass", BuildingType.Grass},
            {"groyne", BuildingType.Groyne},
            {"hanami", BuildingType.Hanami},
            {"hospital", BuildingType.Hospital},
            {"ındustrial", BuildingType.Industrial},
            {"land", BuildingType.Land},
            {"library", BuildingType.Library},
            {"maze", BuildingType.Maze},
            {"meadow", BuildingType.Meadow},
            {"military", BuildingType.Military},
            {"national_park", BuildingType.National_park},
            {"nature_reserve", BuildingType.Nature_reserve},
            {"park", BuildingType.Park},
            {"protected_land", BuildingType.Protected_land},
            {"parking", BuildingType.Parking},
            {"pedestrian", BuildingType.Pedestrian},
            {"petting_zoo", BuildingType.Petting_zoo},
            {"picnic_site", BuildingType.Picnic_site},
            {"pier", BuildingType.Pier},
            {"pitch", BuildingType.Pitch},
            {"place_of_worship", BuildingType.Place_of_worship},
            {"plant", BuildingType.Plant},
            {"playground", BuildingType.Playground},
            {"prison", BuildingType.Prison},
            {"protected_area", BuildingType.Protected_area},
            {"quarry", BuildingType.Quarry},
            {"railway", BuildingType.Railway},
            {"recreation_ground", BuildingType.Recreation_ground},
            {"recreation_track", BuildingType.Recreation_track},
            {"residential", BuildingType.Residential},
            {"resort", BuildingType.Resort},
            {"retail", BuildingType.Retail},
            {"rock", BuildingType.Rock},
            {"roller_coaster", BuildingType.Roller_coaster},
            {"runway", BuildingType.Runway},
            {"rural", BuildingType.Rural},
            {"school", BuildingType.School},
            {"scree", BuildingType.Scree},
            {"scrub", BuildingType.Scrub},
            {"sports_centre", BuildingType.Sports_centre},
            {"stadium", BuildingType.Stadium},
            {"stone", BuildingType.Stone},
            {"substation", BuildingType.Substation},
            {"summer_toboggan", BuildingType.Summer_toboggan},
            {"taxiway", BuildingType.Taxiway},
            {"theatre", BuildingType.Theatre},
            {"theme_park", BuildingType.Theme_park},
            {"tower", BuildingType.Tower},
            {"trail_riding_station", BuildingType.Trail_riding_station},
            {"university", BuildingType.University},
            {"urban_area", BuildingType.Urban_area},
            {"urban", BuildingType.Urban},
            {"village_green", BuildingType.Village_green},
            {"wastewater_plant", BuildingType.Wastewater_plant},
            {"water_park", BuildingType.Water_park},
            {"water_slide", BuildingType.Water_slide},
            {"water_works", BuildingType.Water_works},
            {"wetland", BuildingType.Wetland},
            {"wilderness_hut", BuildingType.Wilderness_hut},
            {"wildlife_park", BuildingType.Wildlife_park},
            {"winery", BuildingType.Winery},
            {"winter_sports", BuildingType.Winter_sports},
            {"wood", BuildingType.Wood},
            {"works", BuildingType.Works},
            {"zoo", BuildingType.Zoo}
        };


        public static int ManhattanTo(this Vector2 v, Vector2 t)
        {
            return (int)Math.Abs(v.x - t.x) + (int)Math.Abs(v.y - t.y);
        }

        public static int ManhattanTo(this Vector2d v, Vector2d t)
        {
            return (int)Math.Abs(v.x - t.x) + (int)Math.Abs(v.y - t.y);
        }

        public static Vector2 ToVector2xz(this Vector3 v)
        {
            return new Vector2(v.x, v.z);
        }

        public static Vector3 ToVector3xz(this Vector2 v)
        {
            return new Vector3(v.x, 0, v.y);
        }

        public static Vector2d ToVector2xz(this Vector3d v)
        {
            return new Vector2d(v.x, v.z);
        }

        public static Vector3d ToVector3xz(this Vector2d v)
        {
            return new Vector3d(v.x, 0, v.y);
        }

        public static Vector2 ToVector2(this Vector3d v)
        {
            return new Vector2((float)v.x, (float)v.z);
        }

        public static Vector2 ToVector2(this Vector2d v)
        {
            return new Vector2((float)v.x, (float)v.y);
        }

        public static Vector2d ToVector2d(this Vector2 v)
        {
            return new Vector2d(v.x, v.y);
        }

        public static Vector3 ToVector3(this Vector2d v)
        {
            return new Vector3((float)v.x, 0, (float)v.y);
        }

        public static Vector2 LatLonToTile(this Vector2 v, int zoom)
        {
            Vector2 p = new Vector2();
            p.x = (float)((v.x + 180.0) / 360.0 * (1 << zoom));
            p.y = (float)((1.0 - Math.Log(Math.Tan(v.y * Math.PI / 180.0) +
                1.0 / Math.Cos(v.y * Math.PI / 180.0)) / Math.PI) / 2.0 * (1 << zoom));

            return p;
        }

        public static Vector2 TileToLatLon(this Vector2 v, int zoom)
        {
            Vector2 p = new Vector2();
            double n = Math.PI - ((2.0 * Math.PI * v.y) / Math.Pow(2.0, zoom));

            p.x = (float)((v.x / Math.Pow(2.0, zoom) * 360.0) - 180.0);
            p.y = (float)(180.0 / Math.PI * Math.Atan(Math.Sinh(n)));

            return p;
        }

        //public static T ConvertToEnum<T>(this string value) where T : new()
        //{
        //    if (!typeof(T).IsEnum)
        //        throw new NotSupportedException("T must be an Enum");

        //    try
        //    {
        //        return (T)Enum.Parse(typeof(T), value, true);
        //    }
        //    catch
        //    {
        //        return default(T); // equivalent to (T)0
        //                           //return (T)Enum.Parse(typeof(T), "Unknown"));
        //    }
        //}

        public static BuildingType ConvertToBuildingType(this string value)
        {
            if (BuildingTypes.ContainsKey(value))
                return BuildingTypes[value];
            return BuildingType.Unknown;
        }

        public static LanduseKind ConvertToLanduseType(this string value)
        {
            if (LanduseKinds.ContainsKey(value))
                return LanduseKinds[value];
            return LanduseKind.Park;
        }

        public static RailwayType ConvertToRailwayType(this string value)
        {
            if (RailwayTypes.ContainsKey(value))
                return RailwayTypes[value];
            return RailwayType.Rail;
        }


        public static RoadType ConvertToRoadType(this string value)
        {
            if (RoadTypes.ContainsKey(value))
                return RoadTypes[value];
            return RoadType.Path;
        }


        public static WaterType ConvertToWaterType(this string value)
        {
            if (WaterTypes.ContainsKey(value))
                return WaterTypes[value];
            return WaterType.Water;
        }


        public static BoundaryType ConvertToBoundaryType(this string value)
        {
            if (BoundaryTypes.ContainsKey(value))
                return BoundaryTypes[value];
            return BoundaryType.Unknown;
        }

        public static EarthType ConvertToEarthType(this string value)
        {
            if (EarthTypes.ContainsKey(value))
                return EarthTypes[value];
            return EarthType.Earth;
        }

        public static PlaceType ConvertToPlaceType(this string value)
        {
            if (PlaceTypes.ContainsKey(value))
                return PlaceTypes[value];
            return PlaceType.Unknown;
        }

        public static string Format(this string s, params object[] args)
        {
            return string.Format(s, args);
        }
    }
}
