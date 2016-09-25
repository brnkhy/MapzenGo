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
            {"Borough",PlaceType.Borough},
            {"City",PlaceType.City},
            {"Continent",PlaceType.Continent},
            {"Country",PlaceType.Country},
            {"Farm",PlaceType.Farm},
            {"Hamlet",PlaceType.Hamlet},
            {"HistoricPlace",PlaceType.HistoricPlace},
            {"IsolatedDwelling",PlaceType.IsolatedDwelling},
            {"Locality",PlaceType.Locality},
            {"Macrohood",PlaceType.Macrohood},
            {"MeteorologicalStation",PlaceType.MeteorologicalStation},
            {"Neighbourhood",PlaceType.Neighbourhood},
            {"PopulatedPlace",PlaceType.PopulatedPlace},
            {"Province",PlaceType.Province},
            {"Quarter",PlaceType.Quarter},
            {"ScientificStation",PlaceType.ScientificStation},
            {"State",PlaceType.State},
            {"Suburb",PlaceType.Suburb},
            {"Town",PlaceType.Town},
            {"Village",PlaceType.Village},
        };

        public static Dictionary<string, BoundaryType> BoundaryTypes = new Dictionary<string, BoundaryType>()
        {
            {"Unknown", BoundaryType.Unknown},
            {"Aboriginal_Lands", BoundaryType.Aboriginal_Lands},
            {"City_Wall", BoundaryType.City_Wall},
            {"Country", BoundaryType.Country},
            {"County", BoundaryType.County},
            {"Dam", BoundaryType.Dam},
            {"Disputed", BoundaryType.Disputed},
            {"Fence", BoundaryType.Fence},
            {"Indefinite", BoundaryType.Indefinite},
            {"Indeterminate", BoundaryType.Indeterminate},
            {"Lease_Limit", BoundaryType.Lease_Limit},
            {"Line_Of_Control", BoundaryType.Line_Of_Control},
            {"Macroregion", BoundaryType.Macroregion},
            {"Municipality", BoundaryType.Municipality},
            {"Overlay_Limit", BoundaryType.Overlay_Limit},
            {"Retaining_Wall", BoundaryType.Retaining_Wall},
            {"Snow_Fence", BoundaryType.Snow_Fence},
            {"State", BoundaryType.State}
        };

        public static Dictionary<string, LanduseKind> LanduseKinds = new Dictionary<string, LanduseKind>()
        {
            {"Aerodrome", LanduseKind.Aerodrome},
            {"Allotments", LanduseKind.Allotments},
            {"Amusement_ride", LanduseKind.Amusement_ride},
            {"Animal", LanduseKind.Animal},
            {"Apron", LanduseKind.Apron},
            {"Aquarium", LanduseKind.Aquarium},
            {"Artwork", LanduseKind.Artwork},
            {"Attraction", LanduseKind.Attraction},
            {"Aviary", LanduseKind.Aviary},
            {"Battlefield", LanduseKind.Battlefield},
            {"Beach", LanduseKind.Beach},
            {"Breakwater", LanduseKind.Breakwater},
            {"Bridge", LanduseKind.Bridge},
            {"Caravan_site", LanduseKind.Caravan_site},
            {"Carousel", LanduseKind.Carousel},
            {"Cemetery", LanduseKind.Cemetery},
            {"Cinema", LanduseKind.Cinema},
            {"College", LanduseKind.College},
            {"Commercial", LanduseKind.Commercial},
            {"Common", LanduseKind.Common},
            {"Cutline", LanduseKind.Cutline},
            {"Dam", LanduseKind.Dam},
            {"Dike", LanduseKind.Dike},
            {"Dog_park", LanduseKind.Dog_park},
            {"Enclosure", LanduseKind.Enclosure},
            {"Farm", LanduseKind.Farm},
            {"Farmland", LanduseKind.Farmland},
            {"Farmyard", LanduseKind.Farmyard},
            {"Footway", LanduseKind.Footway},
            {"Forest", LanduseKind.Forest},
            {"Fort", LanduseKind.Fort},
            {"Fuel", LanduseKind.Fuel},
            {"Garden", LanduseKind.Garden},
            {"Generator", LanduseKind.Generator},
            {"Glacier", LanduseKind.Glacier},
            {"Golf_course", LanduseKind.Golf_course},
            {"Grass", LanduseKind.Grass},
            {"Groyne", LanduseKind.Groyne},
            {"Hanami", LanduseKind.Hanami},
            {"Hospital", LanduseKind.Hospital},
            {"Industrial", LanduseKind.Industrial},
            {"Land", LanduseKind.Land},
            {"Library", LanduseKind.Library},
            {"Maze", LanduseKind.Maze},
            {"Meadow", LanduseKind.Meadow},
            {"Military", LanduseKind.Military},
            {"National_park", LanduseKind.National_park},
            {"Nature_reserve", LanduseKind.Nature_reserve},
            {"Park", LanduseKind.Park},
            {"Protected_land", LanduseKind.Protected_land},
            {"Parking", LanduseKind.Parking},
            {"Pedestrian", LanduseKind.Pedestrian},
            {"Petting_zoo", LanduseKind.Petting_zoo},
            {"Picnic_site", LanduseKind.Picnic_site},
            {"Pier", LanduseKind.Pier},
            {"Pitch", LanduseKind.Pitch},
            {"Place_of_worship", LanduseKind.Place_of_worship},
            {"Plant", LanduseKind.Plant},
            {"Playground", LanduseKind.Playground},
            {"Prison", LanduseKind.Prison},
            {"Protected_area", LanduseKind.Protected_area},
            {"Quarry", LanduseKind.Quarry},
            {"Railway", LanduseKind.Railway},
            {"Recreation_ground", LanduseKind.Recreation_ground},
            {"Recreation_track", LanduseKind.Recreation_track},
            {"Residential", LanduseKind.Residential},
            {"Resort", LanduseKind.Resort},
            {"Retail", LanduseKind.Retail},
            {"Rock", LanduseKind.Rock},
            {"Roller_coaster", LanduseKind.Roller_coaster},
            {"Runway", LanduseKind.Runway},
            {"Rural", LanduseKind.Rural},
            {"School", LanduseKind.School},
            {"Scree", LanduseKind.Scree},
            {"Scrub", LanduseKind.Scrub},
            {"Sports_centre", LanduseKind.Sports_centre},
            {"Stadium", LanduseKind.Stadium},
            {"Stone", LanduseKind.Stone},
            {"Substation", LanduseKind.Substation},
            {"Summer_toboggan", LanduseKind.Summer_toboggan},
            {"Taxiway", LanduseKind.Taxiway},
            {"Theatre", LanduseKind.Theatre},
            {"Theme_park", LanduseKind.Theme_park},
            {"Tower", LanduseKind.Tower},
            {"Trail_riding_station", LanduseKind.Trail_riding_station},
            {"University", LanduseKind.University},
            {"Urban_area", LanduseKind.Urban_area},
            {"Urban", LanduseKind.Urban},
            {"Village_green", LanduseKind.Village_green},
            {"Wastewater_plant", LanduseKind.Wastewater_plant},
            {"Water_park", LanduseKind.Water_park},
            {"Water_slide", LanduseKind.Water_slide},
            {"Water_works", LanduseKind.Water_works},
            {"Wetland", LanduseKind.Wetland},
            {"Wilderness_hut", LanduseKind.Wilderness_hut},
            {"Wildlife_park", LanduseKind.Wildlife_park},
            {"Winery", LanduseKind.Winery},
            {"Winter_sports", LanduseKind.Winter_sports},
            {"Wood", LanduseKind.Wood},
            {"Works", LanduseKind.Works},
            {"Zoo", LanduseKind.Zoo},
        };

        public static Dictionary<string, RailwayType> RailwayTypes = new Dictionary<string, RailwayType>()
        {
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
            {"Basin", WaterType.Basin},
            {"Dock", WaterType.Dock},
            {"Lake", WaterType.Lake},
            {"Ocean", WaterType.Ocean},
            {"Playa", WaterType.Playa},
            {"Riverbank", WaterType.Riverbank},
            {"Swimming_Pool", WaterType.Swimming_Pool},
            {"Water", WaterType.Water},
        };

        public static Dictionary<string, EarthType> EarthTypes = new Dictionary<string, EarthType>()
        {
            {"arete",  EarthType.Arete},
            {"cliff",  EarthType.Cliff},
            {"earth",  EarthType.Earth},
            {"ridge",  EarthType.Ridge},
            {"valley", EarthType.Valley},
        };

        public static Dictionary<string, RoadType> RoadTypes = new Dictionary<string, RoadType>()
        {
            {"Aerialway"  , RoadType.Aerialway},
            {"Exit"       , RoadType.Exit},
            {"Ferry"      , RoadType.Ferry},
            {"Highway"    , RoadType.Highway},
            {"Major_Road" , RoadType.Major_Road},
            {"Minor_Road" , RoadType.Minor_Road},
            {"Path"       , RoadType.Path},
            {"Piste"      , RoadType.Piste},
            {"Racetrack"  , RoadType.Racetrack},
            {"Rail"       , RoadType.Rail}
        };

        public static Dictionary<string, BuildingType> BuildingTypes = new Dictionary<string, BuildingType>()
        {
            {"Unknown", BuildingType.Unknown},
            {"Apartments", BuildingType.Apartments},
            {"Farm", BuildingType.Farm},
            {"Hotel", BuildingType.Hotel},
            {"House", BuildingType.House},
            {"Detached", BuildingType.Detached},
            {"Residential", BuildingType.Residential},
            {"Dormitory", BuildingType.Dormitory},
            {"Terrace", BuildingType.Terrace},
            {"Houseboat", BuildingType.Houseboat},
            {"Bungalow", BuildingType.Bungalow},
            {"StaticCaravan", BuildingType.StaticCaravan},
            {"Commercial", BuildingType.Commercial},
            {"Industrial", BuildingType.Industrial},
            {"Retail", BuildingType.Retail},
            {"Warehouse", BuildingType.Warehouse},
            {"Cathedral", BuildingType.Cathedral},
            {"Chapel", BuildingType.Chapel},
            {"Church", BuildingType.Church},
            {"Mosque", BuildingType.Mosque},
            {"Temple", BuildingType.Temple},
            {"Synagogue", BuildingType.Synagogue},
            {"Shrine", BuildingType.Shrine},
            {"Civic", BuildingType.Civic},
            {"Hospital", BuildingType.Hospital},
            {"School", BuildingType.School},
            {"Stadium", BuildingType.Stadium},
            {"TrainStation", BuildingType.TrainStation},
            {"Transportation", BuildingType.Transportation},
            {"University", BuildingType.University},
            {"Barn", BuildingType.Barn},
            {"Bridge", BuildingType.Bridge},
            {"Bunker", BuildingType.Bunker},
            {"Cabin", BuildingType.Cabin},
            {"Construction", BuildingType.Construction},
            {"Cowshed", BuildingType.Cowshed},
            {"Digester", BuildingType.Digester},
            {"FarmAuxiliary", BuildingType.FarmAuxiliary},
            {"Garage", BuildingType.Garage},
            {"Garages", BuildingType.Garages},
            {"Greenhouse", BuildingType.Greenhouse},
            {"Hangar", BuildingType.Hangar},
            {"Hut", BuildingType.Hut},
            {"Roof", BuildingType.Roof},
            {"Shed", BuildingType.Shed},
            {"Stable", BuildingType.Stable},
            {"Sty", BuildingType.Sty},
            {"TransformerTower", BuildingType.TransformerTower},
            {"Service", BuildingType.Service},
            {"Kiosk", BuildingType.Kiosk},
            {"Ruins", BuildingType.Ruins},
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
