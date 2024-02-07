import { StatusBar } from "expo-status-bar";
import React, { useEffect } from "react";
import { SafeAreaView, StyleSheet, Text, View } from "react-native";
import MapView from "react-native-maps";
import * as Location from "expo-location";
import { useRouter } from "expo-router";

export default function App() {
  const router = useRouter();

  useEffect(() => {
    Location.requestForegroundPermissionsAsync().then(() =>
      router.push("/map")
    );
  }, []);

  return (
    <SafeAreaView>
      <View style={styles.container}>
        <Text>Please accept to share your location</Text>
      </View>
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: "center",
    alignItems: "center",
  },
});
