<template>
  <v-app id="inspire">
    <!-- <v-navigation-drawer v-model="drawer" fixed app>
      <v-list dense>
        <v-list-tile @click="">
          <v-list-tile-action>
            <v-icon>home</v-icon>
          </v-list-tile-action>
          <v-list-tile-content>
            <v-list-tile-title>Home</v-list-tile-title>
          </v-list-tile-content>
        </v-list-tile>
        <v-list-tile @click="">
          <v-list-tile-action>
            <v-icon>contact_mail</v-icon>
          </v-list-tile-action>
          <v-list-tile-content>
            <v-list-tile-title>Contact</v-list-tile-title>
          </v-list-tile-content>
        </v-list-tile>
      </v-list>
    </v-navigation-drawer> -->
    <v-toolbar color="indigo" dark fixed app>
      <!-- <v-toolbar-side-icon @click.stop="drawer = !drawer"></v-toolbar-side-icon> -->
      <v-toolbar-title>Gift Exchange Generator</v-toolbar-title>
    </v-toolbar>
    <v-content>
      <v-container fluid>
        <v-layout justify-center align-center>
          <v-flex text-xs-center>
            <v-tabs fixed-tabs>
              <v-tab>
                Exchange Members
              </v-tab>
              <v-tab>
                No Match Members
              </v-tab>
              <v-tab>
                Results
              </v-tab>
              <v-tab-item>
                <exchange-list></exchange-list>
              </v-tab-item>
              <v-tab-item>
                <nomatch-list></nomatch-list>
              </v-tab-item>
              <v-tab-item>
                <match-results></match-results>
              </v-tab-item>
            </v-tabs>
          </v-flex>
        </v-layout>
      </v-container>
    </v-content>
    <v-footer color="indigo" app>
      <span class="white--text">&copy; 2017</span>
    </v-footer>
  </v-app>
</template>

<script>
import exchangeList from "./components/ExchangeList";
import nomatchList from "./components/NoMatchList";
import matchResults from "./components/MatchResults";

export default {
  components: {
    exchangeList,
    nomatchList,
    matchResults
  },
  data: () => ({
    drawer: null
  }),
  props: {
    source: String
  }
};
</script>
