Light Client:
	Marseille :
		11 boulevard beaurivage
		11 Avenue Haifa Marseille

	Toulouse:
		11 cours Rosalind Franklin
		11 Rue Régence
		
	De chez moi vers plytech:
		321 Avenue Jules Grec Antibes
		930 Rte des Colles, 06410 Biot

	REST requests:
		API = "http://localhost:8084/Design_Time_Addresses/RoutingWithBikes/REST/"
		API + "getstations";
		API + "nearestEndStation?lat=" + lat + "&lng=" + lng;
		API + "nearestStartStation?lat=" + lat + "&lng=" + lng;
		API + "path";


Heavy Client:
	SOAP Functions:
		client.GetPosition(address)
		client.FindNearestStationFromStart(startAddressPosition.lat, startAddressPosition.lng);
		client.FindNearestStationFromEnd(endAddressPosition.lat, endAddressPosition.lng);
		client.GetPath(positions);


	Steps : 
		path.features.ToList()
		
Routing with bikes:
	this.client = new WebProxy.RoutingServiceClient(); => appel constructeur de webProxy
	this.allStations = getAllStations().ToList(); => client.getAllStations(); => récupérer la liste de toutes les stations dans le monde
	GetPosition => openStreetMapNomatim.GetPlacesFromAddress(address).Result;
	FindNearestStationFromStart => getStationInfo(station.contract_name, station.number);
	GetPath => openRouteService.PostDirections(positions, profile).Result;
	getAllStations()
	
	
Test de performance:
	Heavy Client:
		sans cache : 599,901 ms
		avec cache : 403.094 ms
	Light Client:
		sans cache : 0.80 ms 
		avec cache : 0.60 ms
	
	
	