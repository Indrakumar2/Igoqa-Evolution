import _ from 'lodash';

class ArrayUtil {
    contains(arr, x) {
        const result = false;
        if (!this.isArray(arr)) {
            return result;
        }
        const length = arr.length;
        if (length === 0) {
            return result;
        }
        for (let i = 0; i < length; i++) {
            if (arr[i] === x) {
                return true;
            }
        }
        return result;
    }

    isArray(arr) {
        return arr !== undefined && arr.constructor === Array;
    }

    length(arr) {
        let result = 0;
        if (!this.isArray(arr)) {
            return result;
        }
        result = arr.length;
        return result;
    }

    hasNext(arr) {
        let result = false;
        if (!this.isArray(arr)) {
            return result;
        }
        result = arr.length > 0 ? true : false;
        return result;
    }

    shuffle(arr) {
        if (!this.isArray(arr)) {
            return arr;
        }
        const length = arr.length;
        for (let i = 0; i < length; i++) {
            const pos = parseInt(Math.random() * (length - i));
            const save = arr[i];
            arr[i] = arr[pos];
            arr[pos] = save;
        }
        return arr;
    }

    checkPropExists(arr, prop, newVal) {
        if (!this.isArray(arr)) {
            return false;
        }
        return arr.some(function (e) {
            return e[prop] ? e[prop] === newVal : false;
        });
    }

    unique(arr) {
        if (!this.isArray(arr)) {
            return arr;
        }
        const u = [];
        const length = arr.length;
        for (let i = 0; i < length; i++) {
            const o = arr[i];
            if (!this.contains(u, o)) {
                u.push(o);
            }
        }
        return u;
    }

    min(arr) {
        let result = 0;
        if (!this.isArray(arr)) {
            return result;
        }
        const length = arr.length;
        if (length === 0) {
            return result;
        }
        result = arr[0];
        for (let i = 1; i < length; i++) {
            const o = arr[i];
            if (o < result) {
                result = o;
            }
        }
        return result;
    }

    max(arr) {
        let result = 0;
        if (!this.isArray(arr)) {
            return result;
        }
        const length = arr.length;
        if (length === 0) {
            return result;
        }
        result = arr[0];
        for (let i = 1; i < length; i++) {
            const o = arr[i];
            if (o > result) {
                result = o;
            }
        }
        return result;
    }
    filter(arr, prop, filterVal) {

        const result = [];
        if (!this.isArray(arr)) {
            return result;
        }
        const length = arr.length;
        if (length === 0) {
            return result;
        }
        return arr.filter(function (obj) {
            return obj[prop] === filterVal;
        });
    }
    //filters the array and returns object
    FilterGetObject(arr, prop, filterVal) {

        const result = null;
        if (!this.isArray(arr)) {
            return result;
        }
        const length = arr.length;
        if (length === 0) {
            return result;
        }

        for (let i = 0; i < length; i++) {
            if (arr[i][prop] === filterVal) {
                return arr[i];
            }
        }
        return result;
    }

    findIndex(arr, prop, findVal) {
        for (let i = 0; i < arr.length; i++) {
            if (arr[i][prop] === findVal) {
                return i;
            }
        }
        return -1;
    }

    sort(arr, prop, sortOrder) {
        const result = _.orderBy(arr, [ x=>x[prop].toLowerCase() ], [ sortOrder ]);

        return result;
    }

    //function that returns non matched records
    negateFilter =(arr, propVal, itemsToRemove = [])=>{
        return arr.filter((eachItem) => {
               return !itemsToRemove.includes(eachItem[propVal]);
            });
    };
    removeDuplicates = (originalArray, prop) =>{
        const newArray = [];
        const lookupObject  = {};
   
        for(const i in originalArray) {
           lookupObject[originalArray[i][prop]] = originalArray[i];
        }
   
        for(const i in lookupObject) {
            newArray.push(lookupObject[i]);
        }
         return newArray;
    }
    bubbleSort = (array, prop) => {
        let done = false;
        while (!done) {
          done = true;
          for (let i = 1; i < array.length; i += 1) {
            if (array[i - 1][prop] < array[i][prop]) {
              done = false;
              array[i - 1] = [ array[i], array[i]=array[i - 1] ][0];
            }
          }
        }
        return array;
      }

    gridTopSort=(array)=>{
        const sortedArray = [];
        if (array) {
            array.forEach(item => { 
              if(item.recordStatus === "N"){
                sortedArray.push(item); 
              }
            });
            array.forEach(item  =>{
                if(item.recordStatus === "M" || item.recordStatus === null){
                    sortedArray.push(item); 
                  }
            });
        }
        return sortedArray;
    }

    boolsort(arr,prop,sortOrder){
        const result = arr.sort(function(x, y) {
            if(sortOrder === 'asc')
                return x[prop] - y[prop]; 
            else if(sortOrder === 'desc')
                return y[prop] - x[prop];
            });
        return result;
    }
}
export const updateObjectInArray = (array, action) =>{
    const actionIndex = array.findIndex(iteratedValue => {
        return iteratedValue[action.checkProperty]
            === action.editedItem[action.checkProperty];
    });
    return array.map((item, index) => {
        if (index !== actionIndex) {
            // This isn't the item we care about - keep it as-is
            return item;
        }

        // Otherwise, this is the one we want - return an updated value
        return {
            ...item,
            ...action.editedItem
        };
    });
};

export function customComparator(valueA, valueB, nodeA, nodeB, isInverted) { //Added for D314 Grid Sorting issue
    if (!valueA) {
        valueA = '';
    }
    if (!valueB) {
        valueB = '';
    }
   
    if ((typeof valueA === 'string' && typeof valueB === 'string'))
        return valueA.localeCompare(valueB);
    else
        return valueA - valueB;

};

const arrayUtil = new ArrayUtil();
export default arrayUtil;
