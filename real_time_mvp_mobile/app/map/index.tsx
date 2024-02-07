import { StatusBar } from "expo-status-bar";
import React, { useCallback, useEffect, useRef, useState } from "react";
import { SafeAreaView, StyleSheet, Text, View } from "react-native";
import MapView, { Marker } from "react-native-maps";
import * as Location from "expo-location";
import throttle from "../../utils/throttle";
import EstimatedArrivalService from "../../services/estimatedArrivalService";
import { useInterval } from "../../hooks/useInterval";

export default function Map() {
    const initialValues = {
        latitude: 18.474966,
        longitude: -69.90921,
    };

    const [distance, setDistance] = useState(0);
    const [coordinates, setCoordinates] = useState(initialValues);

    const service = new EstimatedArrivalService();

    const locationCallback: Location.LocationCallback = useCallback(
        (location) => {
            service.sendLocation(location.coords, (data) => {
                setCoordinates({
                    longitude: parseFloat(data.longitude),
                    latitude: parseFloat(data.latitude),
                });
                setDistance(parseFloat(data.distance));
            });
        },
        []
    );

    useEffect(() => {
        Location.watchPositionAsync(
            {
                distanceInterval: 1,
                accuracy: Location.LocationAccuracy.Low,
                timeInterval: 500,
            },
            locationCallback
        );
    }, []);

    return (
        <View style={styles.container}>
            <Text>{`distance to downtown is ${distance} m `}</Text>
            <MapView
                style={styles.map}
                region={{
                    latitude: 37.3230,
                    longitude: -122.0322,
                    longitudeDelta: 0.1,
                    latitudeDelta: 0.1,
                }}
            >
                <Marker
                    coordinate={coordinates}
                    title="Jorge 🚴"
                    description="Jorge is trying to arrive to Japan"
                />
            </MapView>
        </View>
    );
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
    },
    map: {
        width: "100%",
        height: "90%",
    },
});
