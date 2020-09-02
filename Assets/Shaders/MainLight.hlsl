void MainLight_half(float3 WorldPos, out half3 Direction, out half3 Color, out half DistanceAtten, out half ShadowAtten)
{
    //#significa que es un grafo
    #if SHADERGRAPH_PREVIEW //una macro es una oalabra reservada que tiene operaciones 
        Direction = half3(0.5, 0.5, 0);
        Color = 1;
        DistanceAtten = 1;
        ShadowAtten = 1;
    #else
        #if SHADOWS_SCREEN //sig. si la app es capaz de representar sobras en la pantalla
            half4 clipPos = TransformWorldToHClip(WorldPos); //de donde se esta viendo (la camara) y se cañcula desde el worlpositicon que es la pisicion del objeto
            half4 shadowCoord = ComputeScreenPos(clipPos);//calcula las coords de la sombra-agarra el cuadro de la camara y saca las coord para reposicionarlo en la pantalla
        #else
            half4 shadowCoord = TransformWorldToShadowCoord(WorldPos); //es lo mismo que lo de arriba pero se ve de peor calidad
        #endif
            Light mainLight = GetMainLight(shadowCoord); //regresa un valor tipo light, coords de la luz principal
            Direction = mainLight.direction; //la direccion
            Color = mainLight.color;//el color
            DistanceAtten = mainLight.distanceAttenuation;//el largo de la luz
        #if !defined(_MAIN_LIGHT_SHADOWS) || defined(_RECEIVE_SHADOWS_OFF) ///primero main light no esta deficina o el recieve shadows esta en off entonces la intensidad sera penetrante
            ShadowAtten = 1.0h; //se ve feo porque se ve profundo
        #endif
        #if SHADOWS_SCREEN //si esta activo esto 
            ShadowAtten = SampleScreenSpaceShadowmap(shadowCoord); //se que tan fuerte es dependiendo de un sampleo que se hace. regresa intensidad
        #else
            ShadowSamplingData shadowSamplingData = GetMainLightShadowSamplingData(); //regresa una sombra
            half shadowStrength = GetMainLightShadowStrength();//regresa luces
            ShadowAtten = SampleShadowmap(shadowCoord, TEXTURE2D_ARGS(_MainLightShadowmapTexture,
            sampler_MainLightShadowmapTexture),
            shadowSamplingData, shadowStrength, false); //convierte a una textura 2D pasandole los parametros
            //e indica que lo que calcula es uan textura de esa, la segunda samplea como un mapa de sombras y
            //que resulte un mapa de sombras y regresa un half con la intensidad de sombra.
            //manda el sample y la fuerza de intensidad 
        #endif
    #endif
}
