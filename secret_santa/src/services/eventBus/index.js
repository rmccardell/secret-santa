import Vue from "vue";
export const Events = {
  EXCHANGE_MEMBER_ADDED: "exchange-member-added",
  EXCHANGE_MEMBER_REMOVED: "exchange-member-removed"
};

export const EventBus = new Vue();

export default {
  Events: Events,
  EventBus: EventBus
};
