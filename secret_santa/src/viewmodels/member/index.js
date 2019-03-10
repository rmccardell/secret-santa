const MemberViewModel = function(member) {
  var self = this;
  self.name = member.name;
  self.id = member.id;
  self.busy = false;
  self.selectiveMatch = member.selectiveMatch;
};

export default MemberViewModel;
