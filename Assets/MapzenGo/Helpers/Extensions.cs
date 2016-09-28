using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.MapzenGo.Models.Enums;
using MapzenGo.Models.Enums;
using UnityEngine;

namespace MapzenGo.Helpers
{
    public static class Extensions
    {
        public static Dictionary<string, PoiType> PoiTypes = new Dictionary<string, PoiType>()
        {
            {"accountant", PoiType.Accountant},
            {"adit", PoiType.Adit},
            {"administrative", PoiType.Administrative},
            {"advertising_agency", PoiType.AdvertisingAgency},
            {"aerodrome", PoiType.Aerodrome},
            {"airport", PoiType.Airport},
            {"alcohol", PoiType.Alcohol},
            {"alpine_hut", PoiType.AlpineHut},
            {"ambulatory_care", PoiType.AmbulatoryCare},
            {"amusement_ride", PoiType.AmusementRide},
            {"animal", PoiType.Animal},
            {"aquarium", PoiType.Aquarium},
            {"archaeological_site", PoiType.ArchaeologicalSite},
            {"architect", PoiType.Architect},
            {"are_home", PoiType.AreHome},
            {"artwork", PoiType.Artwork},
            {"assisted_living", PoiType.AssistedLiving},
            {"association", PoiType.Association},
            {"atm", PoiType.Atm},
            {"attraction", PoiType.Attraction},
            {"aviary", PoiType.Aviary},
            {"bakery", PoiType.Bakery},
            {"bank", PoiType.Bank},
            {"bar", PoiType.Bar},
            {"battlefield", PoiType.Battlefield},
            {"bbq", PoiType.Bbq},
            {"beach_resort", PoiType.BeachResort},
            {"beach", PoiType.Beach},
            {"beacon", PoiType.Beacon},
            {"bed_and_breakfast", PoiType.BedAndBreakfast},
            {"bench", PoiType.Bench},
            {"bicycle_parking", PoiType.BicycleParking},
            {"bicycle_rental", PoiType.BicycleRental},
            {"bicycle_rental_station", PoiType.BicycleRentalStation},
            {"bicycle_repair_station", PoiType.BicycleRepairStation},
            {"bicycle", PoiType.Bicycle},
            {"bicycle_junction", PoiType.BicycleJunction},
            {"biergarten", PoiType.Biergarten},
            {"block", PoiType.Block},
            {"boat_rental", PoiType.BoatRental},
            {"boat_storage", PoiType.BoatStorage},
            {"bollard", PoiType.Bollard},
            {"books", PoiType.Books},
            {"brewery", PoiType.Brewery},
            {"bus_station", PoiType.BusStation},
            {"bus_stop", PoiType.BusStop},
            {"butcher", PoiType.Butcher},
            {"cafe", PoiType.Cafe},
            {"camp_site", PoiType.CampSite},
            {"car_repair", PoiType.CarRepair},
            {"car_sharing", PoiType.CarSharing},
            {"car", PoiType.Car},
            {"caravan_site", PoiType.CaravanSite},
            {"carousel", PoiType.Carousel},
            {"carpenter", PoiType.Carpenter},
            {"cave_entrance", PoiType.CaveEntrance},
            {"chalet", PoiType.Chalet},
            {"childcare", PoiType.Childcare},
            {"childrens_centre", PoiType.ChildrensCentre},
            {"cinema", PoiType.Cinema},
            {"clinic", PoiType.Clinic},
            {"closed", PoiType.Closed},
            {"clothes", PoiType.Clothes},
            {"club", PoiType.Club},
            {"college", PoiType.College},
            {"communications_tower", PoiType.CommunicationsTower},
            {"community_centre", PoiType.CommunityCentre},
            {"company", PoiType.Company},
            {"computer", PoiType.Computer},
            {"confectionery", PoiType.Confectionery},
            {"consulting", PoiType.Consulting},
            {"convenience", PoiType.Convenience},
            {"courthouse", PoiType.Courthouse},
            {"cross", PoiType.Cross},
            {"cycle_barrier", PoiType.CycleBarrier},
            {"dairy_kitchen", PoiType.DairyKitchen},
            {"dam", PoiType.Dam},
            {"day_care", PoiType.DayCare},
            {"dentist", PoiType.Dentist},
            {"department_store", PoiType.DepartmentStore},
            {"dive_centre", PoiType.DiveCentre},
            {"doctors", PoiType.Doctors},
            {"dog_park", PoiType.DogPark},
            {"doityourself", PoiType.Doityourself},
            {"dressmaker", PoiType.Dressmaker},
            {"drinking_water", PoiType.DrinkingWater},
            {"dry_cleaning", PoiType.DryCleaning},
            {"dune", PoiType.Dune},
            {"educational_institution", PoiType.EducationalInstitution},
            {"egress", PoiType.Egress},
            {"electrician", PoiType.Electrician},
            {"electronics", PoiType.Electronics},
            {"embassy", PoiType.Embassy},
            {"emergency_phone", PoiType.EmergencyPhone},
            {"employment_agency", PoiType.EmploymentAgency},
            {"enclosure", PoiType.Enclosure},
            {"estate_agent", PoiType.EstateAgent},
            {"fashion", PoiType.Fashion},
            {"fast_food", PoiType.FastFood},
            {"ferry_terminal", PoiType.FerryTerminal},
            {"financial", PoiType.Financial},
            {"fire_station", PoiType.FireStation},
            {"firepit", PoiType.Firepit},
            {"fishing", PoiType.Fishing},
            {"fishing_area", PoiType.FishingArea},
            {"fitness_station", PoiType.FitnessStation},
            {"fitness", PoiType.Fitness},
            {"florist", PoiType.Florist},
            {"food_bank", PoiType.FoodBank},
            {"ford", PoiType.Ford},
            {"fort", PoiType.Fort},
            {"foundation", PoiType.Foundation},
            {"fuel", PoiType.Fuel},
            {"gardener", PoiType.Gardener},
            {"gas", PoiType.Gas},
            {"gate", PoiType.Gate},
            {"generator", PoiType.Generator},
            {"geyser", PoiType.Geyser},
            {"gift", PoiType.Gift},
            {"government", PoiType.Government},
            {"greengrocer", PoiType.Greengrocer},
            {"group_home", PoiType.GroupHome},
            {"guest_house", PoiType.GuestHouse},
            {"hairdresser", PoiType.Hairdresser},
            {"halt", PoiType.Halt},
            {"hanami", PoiType.Hanami},
            {"handicraft", PoiType.Handicraft},
            {"hardware", PoiType.Hardware},
            {"hazard", PoiType.Hazard},
            {"healthcare", PoiType.Healthcare},
            {"helipad", PoiType.Helipad},
            {"historical", PoiType.Historical},
            {"hospital", PoiType.Hospital},
            {"hostel", PoiType.Hostel},
            {"hot_spring", PoiType.HotSpring},
            {"hotel", PoiType.Hotel},
            {"hunting", PoiType.Hunting},
            {"hvac", PoiType.Hvac},
            {"ice_cream", PoiType.IceCream},
            {"information", PoiType.Information},
            {"insurance", PoiType.Insurance},
            {"it", PoiType.It},
            {"jewelry", PoiType.Jewelry},
            {"kindergarten", PoiType.Kindergarten},
            {"landmark", PoiType.Landmark},
            {"laundry", PoiType.Laundry},
            {"lawyer", PoiType.Lawyer},
            {"level_crossing", PoiType.LevelCrossing},
            {"library", PoiType.Library},
            {"life_ring", PoiType.LifeRing},
            {"lifeguard_tower", PoiType.LifeguardTower},
            {"lift_gate", PoiType.LiftGate},
            {"lighthouse", PoiType.Lighthouse},
            {"lock", PoiType.Lock},
            {"mall", PoiType.Mall},
            {"marina", PoiType.Marina},
            {"mast", PoiType.Mast},
            {"maze", PoiType.Maze},
            {"memorial", PoiType.Memorial},
            {"metal_construction", PoiType.MetalConstruction},
            {"midwife", PoiType.Midwife},
            {"mineshaft", PoiType.Mineshaft},
            {"mini_roundabout", PoiType.MiniRoundabout},
            {"mobile_phone", PoiType.MobilePhone},
            {"monument", PoiType.Monument},
            {"motel", PoiType.Motel},
            {"motorcycle", PoiType.Motorcycle},
            {"motorway_junction", PoiType.MotorwayJunction},
            {"museum", PoiType.Museum},
            {"music", PoiType.Music},
            {"newspaper", PoiType.Newspaper},
            {"ngo", PoiType.Ngo},
            {"notary", PoiType.Notary},
            {"nursing_home", PoiType.NursingHome},
            {"observatory", PoiType.Observatory},
            {"offshore_platform", PoiType.OffshorePlatform},
            {"optician", PoiType.Optician},
            {"outdoor", PoiType.Outdoor},
            {"outreach", PoiType.Outreach},
            {"painter", PoiType.Painter},
            {"parking", PoiType.Parking},
            {"peak", PoiType.Peak},
            {"pet", PoiType.Pet},
            {"petroleum_well", PoiType.PetroleumWell},
            {"petting_zoo", PoiType.PettingZoo},
            {"pharmacy", PoiType.Pharmacy},
            {"phone", PoiType.Phone},
            {"photographer", PoiType.Photographer},
            {"photographic_laboratory", PoiType.PhotographicLaboratory},
            {"physician", PoiType.Physician},
            {"picnic_site", PoiType.PicnicSite},
            {"picnic_table", PoiType.PicnicTable},
            {"place_of_worship", PoiType.PlaceOfWorship},
            {"playground", PoiType.Playground},
            {"plumber", PoiType.Plumber},
            {"police", PoiType.Police},
            {"political_party", PoiType.PoliticalParty},
            {"post_box", PoiType.PostBox},
            {"post_office", PoiType.PostOffice},
            {"pottery", PoiType.Pottery},
            {"power_pole", PoiType.PowerPole},
            {"power_tower", PoiType.PowerTower},
            {"power_wind", PoiType.PowerWind},
            {"prison", PoiType.Prison},
            {"pub", PoiType.Pub},
            {"put_in_egress", PoiType.PutInEgress},
            {"put_in", PoiType.PutIn},
            {"pylon", PoiType.Pylon},
            {"ranger_station", PoiType.RangerStation},
            {"rapid", PoiType.Rapid},
            {"recreation_track", PoiType.RecreationTrack},
            {"recycling", PoiType.Recycling},
            {"refugee_camp", PoiType.RefugeeCamp},
            {"religion", PoiType.Religion},
            {"research", PoiType.Research},
            {"residential_home", PoiType.ResidentialHome},
            {"resort", PoiType.Resort},
            {"restaurant", PoiType.Restaurant},
            {"rock", PoiType.Rock},
            {"roller_coaster", PoiType.RollerCoaster},
            {"saddle", PoiType.Saddle},
            {"sawmill", PoiType.Sawmill},
            {"school", PoiType.School},
            {"scuba_diving", PoiType.ScubaDiving},
            {"shelter", PoiType.Shelter},
            {"shoemaker", PoiType.Shoemaker},
            {"shower", PoiType.Shower},
            {"sinkhole", PoiType.Sinkhole},
            {"ski_rental", PoiType.SkiRental},
            {"ski_school", PoiType.SkiSchool},
            {"ski", PoiType.Ski},
            {"slipway", PoiType.Slipway},
            {"snow_cannon", PoiType.SnowCannon},
            {"social_facility", PoiType.SocialFacility},
            {"soup_kitchen", PoiType.SoupKitchen},
            {"sports_centre", PoiType.SportsCentre},
            {"sports", PoiType.Sports},
            {"spring", PoiType.Spring},
            {"stadium", PoiType.Stadium},
            {"station", PoiType.Station},
            {"stone", PoiType.Stone},
            {"stonemason", PoiType.Stonemason},
            {"subway_entrance", PoiType.SubwayEntrance},
            {"summer_camp", PoiType.SummerCamp},
            {"summer_toboggan", PoiType.SummerToboggan},
            {"supermarket", PoiType.Supermarket},
            {"swimming_area", PoiType.SwimmingArea},
            {"tailor", PoiType.Tailor},
            {"tax_advisor", PoiType.TaxAdvisor},
            {"telecommunication", PoiType.Telecommunication},
            {"telephone", PoiType.Telephone},
            {"telescope", PoiType.Telescope},
            {"theatre", PoiType.Theatre},
            {"theme_park", PoiType.ThemePark},
            {"therapist", PoiType.Therapist},
            {"toilets", PoiType.Toilets},
            {"townhall", PoiType.Townhall},
            {"toys", PoiType.Toys},
            {"trade", PoiType.Trade},
            {"traffic_signals", PoiType.TrafficSignals},
            {"trail_riding_station", PoiType.TrailRidingStation},
            {"trailhead", PoiType.Trailhead},
            {"tram_stop", PoiType.TramStop},
            {"travel_agent", PoiType.TravelAgent},
            {"tree", PoiType.Tree},
            {"university", PoiType.University},
            {"veterinary", PoiType.Veterinary},
            {"viewpoint", PoiType.Viewpoint},
            {"volcano", PoiType.Volcano},
            {"walking_junction", PoiType.WalkingJunction},
            {"waste_basket", PoiType.WasteBasket},
            {"waste_disposal", PoiType.WasteDisposal},
            {"water_park", PoiType.WaterPark},
            {"water_point", PoiType.WaterPoint},
            {"water_slide", PoiType.WaterSlide},
            {"water_tower", PoiType.WaterTower},
            {"water_well", PoiType.WaterWell},
            {"waterfall", PoiType.Waterfall},
            {"watering_place", PoiType.WateringPlace},
            {"wilderness_hut", PoiType.WildernessHut},
            {"wildlife_park", PoiType.WildlifePark},
            {"windmill", PoiType.Windmill},
            {"wine", PoiType.Wine},
            {"winery", PoiType.Winery},
            {"workshop", PoiType.Workshop},
            {"zoo", PoiType.Zoo}
        };

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

        public static PoiType ConvertToPoiType(this string value)
        {
            if (PoiTypes.ContainsKey(value))
                return PoiTypes[value];
            return PoiType.Unknown;
        }

        public static string Format(this string s, params object[] args)
        {
            return string.Format(s, args);
        }
    }
}
