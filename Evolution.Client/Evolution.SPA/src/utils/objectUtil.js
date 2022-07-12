import { cloneDeep,keys,get } from 'lodash';

class ObjectUtil {
    cloneDeep(obj) {
        return cloneDeep(obj);
    }
    fetchKey(obj, index) {
        return keys(obj)[index];
    }

    mapObject(object, callback) {
        return Object.keys(object).map(function (key, index) {
            return callback(index, key, object[key]);
        });
    }
    isEmpty(obj){
        for (const x in obj) { if (obj.hasOwnProperty(x))  return false; }
        return true;  
    }
    getValue(obj, path){
        return get(obj, path);
    }
    keys(objKeys){
        const result = Object.keys(objKeys);
        return result;
    }
}
  /**
     * This method will validate the object that any 
     * @param {*} obj object to validate
     * @param {*} arr array of keys to validate on obj if any key has value 
     */
export function validateObjectHasValue(obj, arr) {
    if (objectUtil.isEmpty(obj) || !Array.isArray(arr) || arr.length === 0) {
        return false;
    }
    return arr.some(key => obj[key]);
}

const objectUtil = new ObjectUtil();
export default objectUtil;