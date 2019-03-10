const Utils = (function() {
  const isPlainObject = function(obj) {
    return Object.prototype.toString.call(obj) === "[object Object]";
  };

  const find = function(array, item, prop = null) {
    if (!(array instanceof Array)) {
      throw new Error(`invalid operation ${array} must be of type Array`);
    }

    var foundItem = array.find(function(element) {
      const elementOrElementProp = prop == null ? element : element[prop];
      const itemOrItemProp = !isPlainObject(item) ? item : item[prop];
      return elementOrElementProp === itemOrItemProp;
    });

    return foundItem;
  };

  const remove = function(array, item, prop = null) {
    if (!(array instanceof Array)) {
      throw new Error(`invalid operation ${array} must be of type Array`);
    }

    var itemNdx = array.findIndex(element => {
      const elementOrElementProp = prop == null ? element : element[prop];
      const itemOrItemProp = !isPlainObject(item) ? item : item[prop];
      return elementOrElementProp === itemOrItemProp;
    });

    if (itemNdx > -1) {
      array.splice(itemNdx, 1);
    }
  };

  return {
    find: find,
    remove: remove
  };
})();

export default Utils;
