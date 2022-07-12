import React, { Component, Fragment } from 'react';
import PropTypes from 'prop-types';
import Select from "react-select";
import { getlocalizeData, isEmpty, isString,isFunction } from '../../../utils/commonUtils';
import { isArray } from 'util';

class CustomMultiSelect extends Component {
    render() {       
        const {
            label,
            colSize,
            divClassName,
            disabled,
            hasLabel,
            defaultValue,
            optionsList,
            optionLabelName,
            labelClass,
            onFocus,
            onBlur
        } = this.props;

        this.onChangeMultiSelectdValue = (e) => {         
            const fieldValue = e;
            this.props.multiSelectdValue(fieldValue);
        };
        this.setMultiSelectedValue = (opts, val, optionLabelName) => {
            if (!val) {
                return '';
            }
            if (isEmpty(opts)) {
                return '';
            }
            if (isString(val)) {
                val = ',' + val;
                const arr = val.split(',');
                return opts.filter(o => arr.some(val => {
                    return o.value === val;
                }));
            }
            if (isArray(val)) {               
                const updateObj = [];
                val.map((item) => {
                    const newItem = Object.assign({}, item);
                    newItem.value = item[optionLabelName];
                    newItem.label = item[optionLabelName];
                    updateObj.push(newItem);
                });
                return updateObj;
            }
            return val;

        };
        const options =this.props.allowSelectAll ? [ this.props.allOption, ...optionsList ] : optionsList;
    return (
        <Fragment>
            <div className={'col mb-1 ' + divClassName + ' ' + colSize} >
                {

                    hasLabel && <label className={labelClass || ''}>{label}</label>
                }
                <Select
                    isDisabled={disabled}
                    closeMenuOnSelect={false}
                    isMulti={true}
                    options={options}
                    onChange={selected => {
                        if (
                            selected.length > 0 &&
                            selected[selected.length - 1].value === this.props.allOption.value
                        ) {
                            return this.onChangeMultiSelectdValue(optionsList);
                        }
                        return this.onChangeMultiSelectdValue(selected);
                    }}
                    // options={optionsList}
                    //defaultValue property changed as to value property for D699 (Render Issue) 
                    value={this.setMultiSelectedValue(optionsList,
                            defaultValue, optionLabelName)}
                    // onChange={(e) => this.onChangeMultiSelectdValue(e)}
                    onFocus={isFunction(onFocus)?onFocus:()=>{}}
                    onBlur={isFunction(onBlur)?onBlur:()=>{}}
                />
            </div>
        </Fragment>
    );
}
  };
export default CustomMultiSelect;

CustomMultiSelect.propTypes = {
    colSize: PropTypes.string,
    allowSelectAll: PropTypes.bool,
    allOption: PropTypes.shape({
      label: PropTypes.string,
      value: PropTypes.string
    })
};

CustomMultiSelect.defaultProps = {
    colSize: 's12',
    divClassName: '',
    labelClass: '',
    allOption: {
        label: "Select all",
        value: "*"
      }
}; 