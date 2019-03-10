<!-- This component allows users to add/update/edit exchange members -->
<template lang="html">
  <section class="exchangelist">
    <v-card>
      <v-toolbar color="gray" light>
        <v-form ref="form">
          <v-text-field
            full-width
            v-model="currentName"
            :counter="255"
            label="Add Member"
            required
          ></v-text-field>
        </v-form>
        <v-btn
          icon
          color="success"
          @click="addMember()"
          :disabled="this.currentName == ''"
        >
          <v-icon>add</v-icon>
        </v-btn>
      </v-toolbar>

      <v-alert :value="this.addMemberError != ''" type="error" dismissible>
        {{ addMemberError }}
      </v-alert>

      <v-list two-line>
        <template v-for="item in members">
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

            <v-tooltip top>
              <template v-slot:activator="{ on }">
                <v-list-tile-action>
                  <v-btn icon ripple>
                    <v-icon color="blue lighten-1">edit</v-icon>
                  </v-btn>
                </v-list-tile-action>
              </template>
              <span>Add to no match</span>
            </v-tooltip>

            <v-list-tile-action @click="removeMember(item)">
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
import {EventBus, Events} from '../services/eventBus'

  export default  {
    name: 'exchangelist',
    props: [],
    async mounted() {

     try {
          const members = await MemberService.getMembers();
          members.forEach(member => this.members.push(new MemberViewModel(member)));
       } catch (error) {

         // eslint-disable-next-line no-console
         console.log(error);

       }

    },

    computed: {

        addEnabled:function(){

           return this.currentName == '';
        }
    },

    data() {
      return {

         selectedMember: null,
         addMemberError:'',
         currentName: '',
         members: []
      }
    },
    methods: {

 validateField () {
        this.$refs.form.validate()
      },
        addMember: async function(){

           if(this.currentName){

             var nameToAdd = this.currentName.trim();

              if(this.memberExist(nameToAdd))
              {
                  this.addMemberError = 'Member already exists.'
                  return;
              }

              //  this.members.push({
              //    name: nameToAdd
              //  });s

              try {

                  var addedMember = await MemberService.addMember({name: this.currentName });
                  this.currentName = '';
                  this.addMemberError ='';
                  this.members.push(addedMember);
                  // notify interested parties that a member was removed
                  EventBus.$emit(Events.EXCHANGE_MEMBER_ADDED);

              } catch (error) {

                  this.addMemberError = 'error occurred adding new member';
              }


           }
        },

        editMember: function(){

        },

        removeMember: async function(member){

          try {

               member.busy = true;

               var removedMember = await MemberService.removeMember(member);

          if(removedMember)
          {
                 var memberNdx = this.members.findIndex((e)=>{
                    return  e.name === member.name;
               });

             if(memberNdx > -1)
             {
                this.members.splice(memberNdx, 1);
             }

              // notify interested parties that a member was removed
              EventBus.$emit(Events.EXCHANGE_MEMBER_REMOVED, removedMember.id);
          }

          } catch (error) {

             // eslint-disable-next-line no-console
             console.log(error);
          }


     },

        memberExist: function (member){

            return this.members.find(function(element) {
            return  element.name === member;
          });
        }
    }
}
</script>

<style scoped lang="scss">
// .exchangelist {
// }
</style>
