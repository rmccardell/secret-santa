<template lang="html">
  <section class="matchresults">
    <v-card>
      <v-toolbar color="gray" light>
        <v-btn
          color="success"
          @click="generateResults()"
          :disabled="this.busy === true"
        >
          Generate Results
        </v-btn>

        <v-progress-circular
          v-if="busy"
          indeterminate
          color="primary"
        ></v-progress-circular>
      </v-toolbar>

      <v-alert :value="this.errorMsg != ''" type="error" dismissible>
        {{ errorMsg }}
      </v-alert>
      <v-list two-line>
        <template v-for="item in matches">
          <v-list-tile :key="item.giver.id" avatar>
            <v-list-tile-avatar>
              <v-icon>person</v-icon>
            </v-list-tile-avatar>
            <v-list-tile-content>
              <v-list-tile-title v-html="item.giver.name"></v-list-tile-title>
              <span v-if="item.errorMessage == null"
                ><v-icon>arrow_forward</v-icon> {{ item.recipient.name }}</span
              >
              <span v-if="item.errorMessage != null" class="red--text"
                ><v-icon>arrow_forward</v-icon> {{ item.errorMessage }}</span
              >
            </v-list-tile-content>
          </v-list-tile>
        </template>
      </v-list>
    </v-card>
  </section>
</template>

<script lang="js">


import MemberService from '../services/memberService'
import {EventBus, Events} from '../services/eventBus'


export default {

        mounted() {

          //listen for changes on event bus
         EventBus.$on(Events.EXCHANGE_MEMBER_REMOVED, ()=> this.externalChanges = true);
         EventBus.$on(Events.EXCHANGE_MEMBER_ADDED, ()=>this.externalChanges = true);
        },

        data() {
          return {

              busy: false,
              errorMsg: '',
              externalChanges:false,
              matches: []
          }
        },
          methods: {

           async generateResults(){

                try {

                     this.errorMsg = '';
                     this.busy  =true;

                var results =await  MemberService.generateExchangeList();

                this.matches = [];
                results.forEach(element => {

                      if(element.errorMessage != null && this.errorMsg =='')
                      {
                         this.errorMsg = 'One or more participants were not able to be matched with a recipient'
                      }

                      this.matches.push(element);
                });

                this.busy = false;

                } catch (error) {
                  this.errorMsg = 'an error occurred';
                  this.busy = false;
                }
            }
    }
}
</script>

<style scoped></style>
