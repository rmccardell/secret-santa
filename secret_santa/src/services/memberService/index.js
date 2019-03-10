import axios from "axios";

const urlBase = "api/members";

const MemberService = (function() {
  const getMembers = async function() {
    try {
      const response = await axios.get(urlBase);

      if (response.status != 200) {
        throw new Error("service error");
      }

      return response.data;
    } catch (error) {
      throw error;
    }
  };

  const addMember = async function(member) {
    try {
      const response = await axios.post(urlBase, member);

      // eslint-disable-next-line no-console
      console.log(response);

      if (response.status != 201) {
        throw new Error("error occurred adding new member");
      }

      return response.data;
    } catch (error) {
      throw error;
    }
  };

  const removeMember = async function(member) {
    try {
      const response = await axios.delete(`${urlBase}/?id=${member.id}`);

      if (response.status != 200) {
        throw new Error("error occurred deleting member");
      }

      return response.data;
    } catch (error) {
      throw error;
    }
  };

  const updateMember = async function(member) {
    try {
      const response = await axios.put(urlBase, member);

      // eslint-disable-next-line no-console
      console.log(response);

      if (response.status != 200) {
        throw new Error("error occurred adding new member");
      }

      return response.data;
    } catch (error) {
      throw error;
    }
  };

  const generateExchangeList = async function() {
    const response = await axios.get(`${urlBase}/results`);
    if (response.status != 200) {
      throw new Error("error occurred adding new member");
    }

    return response.data;
  };

  return {
    getMembers: getMembers,
    addMember: addMember,
    removeMember: removeMember,
    updateMember: updateMember,
    generateExchangeList: generateExchangeList
  };
})();

export default MemberService;
