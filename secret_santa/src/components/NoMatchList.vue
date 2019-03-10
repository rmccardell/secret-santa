<template lang="html">
  <section class="nomatchlist">
    <v-card>
      <v-toolbar color="gray" light>
        <v-form ref="form">
          <v-select
            v-model="selectedMemberId"
            :items="members"
            label="Select Member"
            item-text="name"
            item-value="id"
          ></v-select>
        </v-form>
        <v-btn
          icon
          color="success"
          @click="addMember()"
          :disabled="this.selectedMemberId == null"
        >
          <v-icon>add</v-icon>
        </v-btn>

        <v-spacer></v-spacer>
        <!-- <v-alert :value="true" type="info" outline>
          Exchange Members added here will be prevented from being matched with one another
        </v-alert> -->

        <v-layout wrap align-right>
          <span class="blue--text">
            <v-icon class="blue--text">info</v-icon>
            Members added to this list will be prevented from being matched with
            one another.
          </span>
        </v-layout>
      </v-toolbar>

      <v-alert :value="this.addMemberError != ''" type="error" dismissible>
        {{ addMemberError }}
      </v-alert>

      <v-alert :value="this.warning != ''" type="warning" dismissible>
        {{ warning }}
      </v-alert>

      <v-list two-line>
        <template v-for="item in noMatchMembers">
          <v-list-tile :key="item.name" avatar>
            <v-list-tile-avatar>
              <v-icon>person</v-icon>
            </v-list-tile-avatar>
            <v-list-tile-content>
              <v-list-tile-title v-html="item.name"></v-list-tile-title>
            </v-list-tile-content>

            <v-progress-circular
              v-if="item.busy"
              indeterminate
              color="primary"
            ></v-progress-circular>

            <v-list-tile-action @click="UpdateMember(item)">
              <v-btn icon ripple>
                <v-icon color="red lighten-1">remove_circle</v-icon>
              </v-btn>
            </v-list-tile-action>
          </v-list-tile>
        </template>
      </v-list>
    </v-card>
  </section>
</template>

<script lang="js">

import MemberService from '../services/memberService'
import MemberViewModel from '../viewmodels/member'
import Utils from '../utils/'
import {EventBus, Events} from '../services/eventBus'

export default {
 name: 'nomatchlist',
    props: [],
     async mounted() {

     try {
         await this.loadMembers();
         EventBus.$on(Events.EXCHANGE_MEMBER_REMOVED, this.removeMember);
         EventBus.$on(Events.EXCHANGE_MEMBER_ADDED, this.loadMembers);
       } catch (error) {

         // eslint-disable-next-line no-console
         console.log(error);

       }

    },
  data() {
    return {

      selectedMemberId: null,
      addMemberError:'',
      warning: '',
      members: [],
      noMatchMembers: [],

    };
  },

  methods: {

       async loadMembers(){

          const members = await MemberService.getMembers();
          this.members = [];
          this.noMatchMembers = [];
          members.forEach(m=>{

                let member= new MemberViewModel(m);

                this.members.push(member);

                 if(m.selectiveMatch){
                     this.noMatchMembers.push(member);
                 }
          });

                    this.validate();
       },
       async addMember(){

          try {

           let selected = this.selectedMemberId;

           if(selected != null)
           {
               var member = Utils.find(this.members, selected, 'id');
               var memberAlreadyInNoMatch = Utils.find(this.noMatchMembers, selected, 'id');

             if(memberAlreadyInNoMatch != null){

                 this.addMemberError = 'Member already in list'
                 return;
             }

              await this.UpdateMember(member);
           }

          } catch (error) {

              this.addMemberError = 'an error occured updated member';
          }
       },
        UpdateMember: async function(member){


          try {


               member.selectiveMatch = !member.selectiveMatch;
               member.busy = true;
               var updatedMember = await MemberService.updateMember(member);
               member.busy = false;

          if(updatedMember.selectiveMatch == false)
          {
               var memberInMembersList = Utils.find(this.members, updatedMember, 'name');
               memberInMembersList.selectiveMatch = updatedMember.selectiveMatch;
               this.removeMember(updatedMember.id);
                this.validate();

               return;
          }else{

            this.noMatchMembers.push(new MemberViewModel(updatedMember));

          }

          this.validate();

          } catch (error) {

             member.busy = false;
             // eslint-disable-next-line no-console
             console.log(error);
          }

     },

        memberExist: function (memberId){

            return this.noMatchMembers.find(function(element) {
            return  element.id === memberId;
          });
        },

        removeMember: async function(memberId){

           var item = Utils.find(this.noMatchMembers, memberId, 'id');

               if(item != null)
               {
                   Utils.remove(this.noMatchMembers, memberId, 'id');
               }


          await this.loadMembers();

        },

        validate: function(){

            this.warning = '';
             var memberCount = this.members.length;
             var noMatchCount = this.noMatchMembers.length;
             var ratio = noMatchCount/memberCount;

             if(ratio > 0.5)
             {
                 this.warning = 'The current configurtion will more than likely produce result with one or more participants failing to matched with a gift recipient.'
             }
        }
  },
};
</script>

<style scoped></style>
