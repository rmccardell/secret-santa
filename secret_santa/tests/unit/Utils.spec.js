// import { describe, it } from "mocha";
import assert from "assert";
import Utils from "../../src/utils";

describe("Utils", function() {
  describe("find", function() {
    it("should be able to locate an existing value in an array", function() {
      var primitiveArray = [1, 2, 3];

      var item = Utils.find(primitiveArray, 1);

      assert.notEqual(item, null);
      assert.equal(item, 1);
    });
  });
});
