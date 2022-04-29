# Let's go biking
## Réalisé par AJIMI Ghada
### Le 29/04/2022

#### Remarque :
	J'ai amélioré le projet par rapport à la soutenance:
	- Diviser les couleur du parcour selon le moyen de transport (rouge = pied, bleu = vélo)
	- Fussionner les projets dans une seule solution
	- Ajouter Docker

#### Web proxy:
	Contract : IRoutingService
	- getAllStations() => récupére la liste des stations depuis JCDcaux
	- getStationInfo() => récupére les informations d'une station depuis JCDcaux
		
	Service: RoutingProxy
	- getAllStations() => récupére la liste des stations depuis le cache
	- getStationInfo() => récupére les informations d'une station depuis le cache
		
	Cache: ProxyCache	
	- Get(string url, double time) => récuprére le cache s'il existe sinon elle le créer.

	Générique: JCDecauxItem
	- JCDecauxItem(string url) => récupére les datas depuis une requête 
		
	Self hosting: Program => contient un main qui permet de lancer le projet
		
	app.config ne contient pas le service car ceci a été déclaré dans le main avec le endpoint et le behavior.	
	
#### Routing with bikes:
	Contract : IRouting
	- Station[] getAllStations();
	- Station getStationInfo(string contrat, int num);
	- Position GetPosition(string address);
	- GeoJson[] GetPath(Position[] positions);
	- Station getStation1(double latitude, double longitude);
	- Station getStation2(double latitude, double longitude);
	- void PathOptions();

	Service: Routing => implémente le comportement des méthodes :
	- this.client = new WebProxy.RoutingServiceClient(); => appel constructeur de webProxy
	- this.allStations = getAllStations().ToList(); => client.getAllStations(); => récupére la liste de toutes les stations depuis webProxy
	- GetPosition => openStreetMapNomatim.GetPlacesFromAddress(address).Result; => récupére l'address la plus importante depuis une chaine de caractére
	- getStation1 => getStationInfo(station.contract_name, station.number); => récupére la station la plus proche de la position de départ
	- GetPath => openRouteService.PostDirections(positions, profile).Result; => récupére les directions à suivre pour atteindre la distination
	
	OpenRouteService
	- async Task<string> PostDirections(string request, string data); => récupére les directions à suivre depuis des position sous forme json (data)
	- async Task<string> PostDirections(Position[] positions, string profile); => récupére les directions à suivre selon le moyen de transport (foot-walking, cycling-regular)
	- string BuildDataForPOSTCall(Position[] positions); => transforme la liste des positions en une chaine json
	
	OpenStreetMap
	- async Task<List<Place>> GetPlacesFromAddress(string address); => récupére la liste des places depuis une adresse
	
	Self hosting: Program => contient un main qui permet de lancer le projet
		
	app.config contient un service avec 3 endpoints différents:
	- <endpoint address="/mex" binding="mexHttpBinding" contract="IMetadataExchange" />
	- <endpoint address="/SOAP" binding="basicHttpBinding" contract="RoutingWithBikes.IRouting" />
	- <endpoint address="/REST" behaviorConfiguration="REST-config" binding="webHttpBinding" contract="RoutingWithBikes.IRouting" />
		
	contient aussi deux comportements différents, un pour le Soap et un pour le REST:
	- <serviceBehaviors>
		<behavior name="SOAP-config">
			<serviceMetadata httpGetEnabled="True" httpsGetEnabled="True"/>
			<serviceDebug includeExceptionDetailInFaults="False" />
		</behavior>
	  </serviceBehaviors> => Soap service
	- <endpointBehaviors> 
		<behavior name="REST-config"> 
			<webHttp/> 
		</behavior>
	  </endpointBehaviors> => Rest endpoint
	
#### Heavy Client:
	Program : 
	- fonction main qui lance le projet
	- void SearchRoute(IRouting client) => permet de récupérer les adresses et calculer la route à suivre
	- void writeSteps(List<Feature> features); => boucle sur les étapes à suivre et les écrit sur le terminal
	- void exit() => tue le processus
		
	Les fonctions SOAP utilisées:
	- client.GetPosition(address)
	- client.getStation1(px1, py1);
	- client.getStation2(px2, py2);
	- client.GetPath(positions);

	Steps : 
	- path.features.ToList();

#### Light Client:
	Marseille :
	- 11 boulevard beaurivage
	- 11 Avenue Haifa Marseille

	Toulouse:
	- 11 cours Rosalind Franklin
	- 11 Rue Régence
		
	De Nice à Paris:
	- Nice
	- Paris

	Les requêtes REST envoyé depuis le light client sont:
	- API = "http://localhost:8084/Design_Time_Addresses/RoutingWithBikes/REST/"
	- API + "getstations";
	- API + "getStation1?lat=" + p1.lat + "&lng=" + p1.lng;
	- API + "getStation2?lat=" + p2.lat + "&lng=" + p2.lng;
	- API + "path";
	
	Steps:
	- Afficher le parcour à suivre selon le moyen de transport.
	- Rouge => à pied
	- Bleu => à vélo


#### Test de performance:
	Heavy Client:
	- sans cache : 599,901 ms
	- avec cache : 403.094 ms
	Light Client:
	- sans cache : 0.80 ms 
	- avec cache : 0.60 ms
	
	
	
