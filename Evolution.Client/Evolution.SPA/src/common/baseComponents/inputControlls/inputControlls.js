import React, { Component, Fragment } from 'react';
import PropTypes from 'prop-types';
import dateUtil from '../../../utils/dateUtil';
import moment from 'moment';
import DatePicker from 'react-datepicker';
import { Portal } from 'react-overlays';
import Select from "react-select";
import { MultiSelect } from 'react-selectize';
import 'react-datepicker/dist/react-datepicker.css';
import { getlocalizeData, isEmpty, isString } from '../../../utils/commonUtils';
import MaskedInput from 'react-text-mask';
import { isArray } from 'util';
const localConstant = getlocalizeData();

const CalendarContainer = ({ children }) => {
   const el = document.getElementById('calendar-portal');
     return (
      <Portal container={el}>
        {children}
      </Portal>
    );
};

class CustomInput extends Component {
    handleDateChangeRaw = e => {        
        this.props.onDateChangeRaw && this.props.onDateChangeRaw(e);
    }
    render() {
        const {
                isVisable,
                inputModal,
                label,
                autocomplete,
                labelName,
                errorMsg,
                type,
                colSize,
                divClassName,
                showPasswordToggle,
                // disabled,
                htmlFor,
                hasLabel,
                prefixIcon,
                max,
                min,
                name,
                step,
                placeholder,
                required,
                inputClass,
                onValueKeypress,
                onValueFocus,
                onValueBlur,
                onValueInput,
                onValueChange,
                defaultValue,
                onSelectChange,
                optionsList,
                optionName,
                optionValue,
                optionGroupLabelKey,
                onSelectGroupChange,
                optionGroups,
                optionKeyValue,
                isSwitchLabel,
                switchLabel,
                switchName,
                checkedStatus,
                onChangeToggle,
                radioGroup,
                radioName,
                isSelectOption, 
                onRadioChange,
                isNonEditDateField,
                subOption,
                checkboxName,
                onCheckboxChange,
                optionsArray,
                checkBoxArray,
                value,
                labelClass,
                onDateChange,
                selectedDate,
                dateFormat,
                onDatePickBlur,
                shouldCloseOnSelect,
                id,
                onRef,
                optionKeyName,
                ref,
                minLength,
                maxLength,
                isChecked,
                refProps,
                onValueKeyDown,
                decimalScale,
                fixedDecimalScale,
                dataValType,
                percentageDisplayValue,
                optionSelecetLabel,
                dataTypePassword,
                //onChangeMultiSelectdValue
                multiSelectdValue,
                placeholderText,           
                autofocus,
                optionClassName,
                optionClassArray,
                optionProperty,
                dataType,
                toFixedlimit,
                valueType,
                isLimitType,
                prefixLimit,
                suffixLimit,
                selectstatus,
                switchKey,
                rownum,
                colnum,
                wraptext,
                disabled,
                readOnly,
                switchInputClass,
                switchSpanClass,
                timeFormat,
            timeIntervals,
            timeCaption,
            isTooltip,
            minTime,
            maxTime,
            onDateSelect,
            customId,
            onValuePaste,//chandra
            title,
            customDefaultValueCheck,
            defaultValueCheckComparator
            } = this.props;

        this.onValueChangeLength = (e) => {
            if (e.currentTarget.value.length >= maxLength) {
                e.currentTarget.value = e.currentTarget.value.slice(0, maxLength);
            }
            // onValueChange(e);        
        };

        this.onNumericKeypress = (event) => {
            const theEvent = event || window.event;
            const keyCode = theEvent.keyCode || theEvent.which;
            const key = String.fromCharCode(keyCode);
            const regex = /[0-9]|\./;
            // const regex= /^[0-9]+(\.[0-9]{1,2})?$/;
            if (!regex.test(key) && keyCode >= 48 && keyCode <= 57) {
                theEvent.returnValue = false;
                if (theEvent.preventDefault) theEvent.preventDefault();
            }
        };

        this.onChangeMultiSelectdValue = (e) => {
            const fieldValue = e;
            this.props.multiSelectdValue(fieldValue);
        };
        this.decimalFormat = (evt) => {
            const limit = toFixedlimit;
            const e = evt || window.event;
            let val = e.target.value.replace(/[^0-9.]/g, '');
            val = val.replace(/(\..*)\./g, '$1');
            e.target.value = val;
            if (e.preventDefault) e.preventDefault();
        };

        this.decimalWithLimtFormat = (evt) => {
            const e = evt || window.event;
            //const match = (/(\d{0,10})[^.]*((?:\.\d{0,6})?)/g).exec(e.target.value.replace(/[^\d.]/g, ''));
            const expression = ("(\\d{0," + parseInt(prefixLimit) + "})[^.]*((?:\\.\\d{0," + parseInt(suffixLimit) + "})?)");
            const rg = new RegExp(expression, "g");
            const match = rg.exec(e.target.value.replace(/[^\d.]/g, ''));
            e.target.value = match[1] + match[2];

            if (e.preventDefault) e.preventDefault();
        };

        this.numericFormat = (evt) => {
            const e = evt || window.event;
            let val = e.target.value.replace(/[^0-9]/g, '');
            val = val.replace(/(\.*)/g, '');
            e.target.value = val;
            if (e.preventDefault) e.preventDefault();
        };
        this.setMultiSelectedValue = (opts, val) => {
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
                return val;
            }
            return val;

        };
        //D699 (Render Issue)
        if (dataValType === 'valuePassword' &&(type === 'text' || type === 'password')){
            const divClassName = this.props.divClassName ? this.props.divClassName : '';

            return (
                <Fragment>
                    <div className={'col mb-1 ' + divClassName + ' ' + colSize} >
                        {hasLabel && <label className={labelClass || ''}>{label}</label>}
                        {prefixIcon && <i className="tiny material-icons prefix customIcon">
                            <img alt='' className="imageStyles" src={prefixIcon} />
                        </i>
                        }
                        <input
                            id={htmlFor}
                            max={max || null}
                            min={min || null}
                            maxLength={maxLength || null}
                            minLength={minLength || null}
                            name={name || null}
                            placeholder={placeholder || null}
                            required={required || null}
                            step={step || null}
                            autoComplete={autocomplete || null}
                            type={(isVisable ? 'text' : type) || 'text'}
                            className={"browser-default text " + inputClass || "browser-default"}
                            onChange={onValueChange || null}
                            onKeyPress={onValueKeypress || null}
                            onKeyDown={type === 'number' ? this.onNumericKeypress : onValueKeyDown || null}
                            onBlur={onValueBlur || null}
                            onFocus={onValueFocus || null}
                            disabled={disabled || null}
                            onInput={onValueInput || null}
                            //defaultValue={defaultValue || null}
                            value={value} //D1281 Fix
                            readOnly={readOnly || null}
                            ref={refProps || null}
                            onPaste={onValuePaste || null}//chandra
                        />
                        {(type === 'password' || dataTypePassword === 'text') && //def 957 fix
                            <i className="tiny material-icons passwordIcon" onClick={ !disabled? showPasswordToggle : null }> 
                                <img alt='' src={isVisable ? "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAALEwAACxMBAJqcGAAAAetJREFUSInt1M2LzlEUB/DPzDNlM+P9ZUzYeKnZaKQkhlIsyUYjfwELC4OxNMnGRhY2UrZiJopQlgqThZfUaKKJEY0SSooZZizueTx3fp7HjKnZyKlbv3vO9/s953fuvYf/NsO2YCbFV2MEPVhWC1Q3iUgzdqEdazA//O8xF62x78Dlv6muFZfwHeOTrAeon6pwA7oxWhAZxD4sxfFCbEvG70Cplvhs3A7SC/TG9wAWYiPOR7Ky+NWMvz18N9BUFG9CXwAuohH3Y78D26r81ah00LlGT8Tu5UnqcT0CZ1X6+TVWCbf83vszmfieTOtcxG8G15FwXDHxsH7gG3biXUH8k8obaMSrjFfCtcAdhs94gzmFtj2rUnV5Hc1w3egvcOdhGB/heVTaVgCdqCE+iFmBacGXSJLbeumM+kkHOBbExYUq3lZJ0JFhLmCo8PfNeCm1eGvZ2RXkJwEo2zoT+9+n8vrbpNauzfAteBrYzvyX6nAqAkPYFP4SHmcJNmec3ViU7dvxOnAnVRlDdTgkjYcx6body8R7i4SwFYEdk/p+sAbul23AIxP7PoKVEW+QXm2n9D7K8+qhdLhTspLKqBjH6Sx2oJD8DvaqMfD+NK6HsUS6y6vwIfxdUcAA7kqXYFq2XLqG+6cr8G/YT2VnofdMiQtDAAAAAElFTkSuQmCC" : "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAQAAABKfvVzAAAABGdBTUEAALGPC/xhBQAAACBjSFJNAAB6JgAAgIQAAPoAAACA6AAAdTAAAOpgAAA6mAAAF3CculE8AAAAAmJLR0QAAKqNIzIAAAAJcEhZcwAADdcAAA3XAUIom3gAAAAHdElNRQfiCAYSNTQeZEUpAAAA6klEQVQ4y+XSMWoCARCF4U9IIZhoZxGbXMBziLDkANEjbG0TUqQQw/Y29jmCJ7AI2FhsLqBYCFYpYhWcNMZsghghpTPd8Abmf284wyodmNU0NZHLvR1fbxiZi0LPjTQOi6v6NsKHMJHJTIStsNFX/S3vWgszLS23+2liZWAmrHWL8p6w0NkRlaRe5VJliaWajoXQ+5JnwlQdN67B/Y4gxUQbdVMhg6EwVgFPHvHi2VoIOTIPoGIsDC9+kLy7wqW742Z+n9Q2Qbo39eBJReiapURZKj8GXbR1YCX529ZicNvTgjv5Nf75fGdSn/ZoeJjRuTkYAAAAJXRFWHRkYXRlOmNyZWF0ZQAyMDE4LTA4LTA2VDE4OjUzOjUyKzAyOjAwsRD/MgAAACV0RVh0ZGF0ZTptb2RpZnkAMjAxOC0wOC0wNlQxODo1Mzo1MiswMjowMMBNR44AAAAZdEVYdFNvZnR3YXJlAHd3dy5pbmtzY2FwZS5vcmeb7jwaAAAAAElFTkSuQmCC"} />
                            </i>
                        }  
                        {errorMsg && <span className="helper-text" data-error={errorMsg}></span>}
                    </div>
                </Fragment>

            );
        }

        if ((type === 'text' && dataValType !== 'valueText' && dataType !== 'numeric' && dataType !== 'decimal' && valueType !== 'value') || type === 'password' || (type === 'number' && dataValType !== 'valueText') || (type === 'email' && dataValType !== 'valueText' && valueType !== 'value')) {
            const divClassName = this.props.divClassName ? this.props.divClassName : '';

            return (
                <Fragment>
                    <div className={'col mb-1 ' + divClassName + ' ' + colSize} >
                        {hasLabel && <label className={labelClass || ''}>{label}</label>}
                        {prefixIcon && <i className="tiny material-icons prefix customIcon">
                            <img alt='' className="imageStyles" src={prefixIcon} />
                        </i>
                        }
                        <input
                            id={htmlFor}
                            max={max || null}
                            min={min || null}
                            maxLength={maxLength || null}
                            minLength={minLength || null}
                            name={name || null}
                            placeholder={placeholder || null}
                            required={required || null}
                            step={step || null}
                            autoComplete={autocomplete || null}
                            type={(isVisable ? 'text' : type) || 'text'}
                            className={"browser-default text " + inputClass || "browser-default"}
                            onChange={onValueChange || null}
                            onKeyPress={onValueKeypress || null}
                            onKeyDown={type === 'number' ? this.onNumericKeypress : onValueKeyDown || null}
                            onBlur={onValueBlur || null}
                            onFocus={onValueFocus || null}
                            disabled={disabled || null}
                            onInput={onValueInput || null}
                            defaultValue={defaultValue || null}
                            //    value={value || null}
                            readOnly={readOnly || null}
                            ref={refProps || null}
                            onPaste={onValuePaste || null}//chandra
                        />
                        {(type === 'password' || dataTypePassword === 'text') &&
                            <i className="tiny material-icons passwordIcon" onClick={showPasswordToggle}>
                                <img alt='' src={isVisable ? "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAALEwAACxMBAJqcGAAAAetJREFUSInt1M2LzlEUB/DPzDNlM+P9ZUzYeKnZaKQkhlIsyUYjfwELC4OxNMnGRhY2UrZiJopQlgqThZfUaKKJEY0SSooZZizueTx3fp7HjKnZyKlbv3vO9/s953fuvYf/NsO2YCbFV2MEPVhWC1Q3iUgzdqEdazA//O8xF62x78Dlv6muFZfwHeOTrAeon6pwA7oxWhAZxD4sxfFCbEvG70Cplvhs3A7SC/TG9wAWYiPOR7Ky+NWMvz18N9BUFG9CXwAuohH3Y78D26r81ah00LlGT8Tu5UnqcT0CZ1X6+TVWCbf83vszmfieTOtcxG8G15FwXDHxsH7gG3biXUH8k8obaMSrjFfCtcAdhs94gzmFtj2rUnV5Hc1w3egvcOdhGB/heVTaVgCdqCE+iFmBacGXSJLbeumM+kkHOBbExYUq3lZJ0JFhLmCo8PfNeCm1eGvZ2RXkJwEo2zoT+9+n8vrbpNauzfAteBrYzvyX6nAqAkPYFP4SHmcJNmec3ViU7dvxOnAnVRlDdTgkjYcx6body8R7i4SwFYEdk/p+sAbul23AIxP7PoKVEW+QXm2n9D7K8+qhdLhTspLKqBjH6Sx2oJD8DvaqMfD+NK6HsUS6y6vwIfxdUcAA7kqXYFq2XLqG+6cr8G/YT2VnofdMiQtDAAAAAElFTkSuQmCC" : "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAQAAABKfvVzAAAABGdBTUEAALGPC/xhBQAAACBjSFJNAAB6JgAAgIQAAPoAAACA6AAAdTAAAOpgAAA6mAAAF3CculE8AAAAAmJLR0QAAKqNIzIAAAAJcEhZcwAADdcAAA3XAUIom3gAAAAHdElNRQfiCAYSNTQeZEUpAAAA6klEQVQ4y+XSMWoCARCF4U9IIZhoZxGbXMBziLDkANEjbG0TUqQQw/Y29jmCJ7AI2FhsLqBYCFYpYhWcNMZsghghpTPd8Abmf284wyodmNU0NZHLvR1fbxiZi0LPjTQOi6v6NsKHMJHJTIStsNFX/S3vWgszLS23+2liZWAmrHWL8p6w0NkRlaRe5VJliaWajoXQ+5JnwlQdN67B/Y4gxUQbdVMhg6EwVgFPHvHi2VoIOTIPoGIsDC9+kLy7wqW742Z+n9Q2Qbo39eBJReiapURZKj8GXbR1YCX529ZicNvTgjv5Nf75fGdSn/ZoeJjRuTkYAAAAJXRFWHRkYXRlOmNyZWF0ZQAyMDE4LTA4LTA2VDE4OjUzOjUyKzAyOjAwsRD/MgAAACV0RVh0ZGF0ZTptb2RpZnkAMjAxOC0wOC0wNlQxODo1Mzo1MiswMjowMMBNR44AAAAZdEVYdFNvZnR3YXJlAHd3dy5pbmtzY2FwZS5vcmeb7jwaAAAAAElFTkSuQmCC"} />
                            </i>
                        }
                        {errorMsg && <span className="helper-text" data-error={errorMsg}></span>}
                    </div>
                </Fragment>

            );
        }

        if (type === 'textarea') {
            const divClassName = this.props.divClassName ? this.props.divClassName : 'input-field';
            const readOnlyClass = readOnly ? "readonlyColor " : "";
            return (
                <Fragment>
                    <div className={'col mb-1 ' + divClassName + ' ' + colSize} >
                        {hasLabel && <label className={labelClass || ''}>{label}</label>}

                        <textarea
                            id={htmlFor}
                            name={name || null}
                            placeholder={placeholder || null}
                            required={required || null}
                            maxLength={maxLength || null}
                            minLength={minLength || null}
                            type='textarea'
                            className={"browser-default " + readOnlyClass + inputClass || "browser-default"}
                            onChange={onValueChange || null}
                            onKeyPress={onValueKeypress || null}
                            onBlur={onValueBlur || null}
                            // disabled={disabled || null}
                            onInput={onValueInput || null}
                            readOnly={readOnly || null}
                            defaultValue={defaultValue || null}
                            ref={ref || null}
                            value={value}
                            cols={colnum}
                            rows={rownum}
                            wrap={wraptext}
                            autoFocus={autofocus || null}
                        />
                    </div>
                </Fragment>

            );
        }
        if (dataValType === 'valueText') {
            const divClassName = this.props.divClassName ? this.props.divClassName : '';

            return (
                <Fragment>
                    <div className={'col mb-1 ' + divClassName + ' ' + colSize} >
                        {hasLabel && <label className={labelClass || ''}>{label}</label>}
                        {prefixIcon && <i className="tiny material-icons prefix customIcon valueType">
                            <img alt='' className="imageStyles" src={prefixIcon} />
                        </i>
                        }
                        <input
                            id={htmlFor}
                            max={max || null}
                            min={min || null}
                            maxLength={maxLength || null}
                            minLength={minLength || null}
                            name={name || null}
                            placeholder={placeholder || null}
                            required={required || null}
                            step={step || null}
                            autoComplete={autocomplete || null}
                            type={(isVisable ? 'text' : type) || 'text'}
                            className={"browser-default emailtextbox " + inputClass || "browser-default"}
                            onChange={type === 'number' ? onValueChange : onValueChange || null}
                            onKeyPress={onValueKeypress || null}
                            onKeyDown={type === 'number' ? this.onNumericKeypress : onValueKeyDown || null}
                            onBlur={onValueBlur || null}
                            onFocus={onValueFocus || null}
                            // disabled={disabled || null}
                            onInput={onValueInput || null}
                            //defaultValue={defaultValue || null }
                            data-value={dataValType || null}
                            value={value}
                            readOnly={readOnly || null}
                            ref={refProps || null}
                            title={title || null }
                        />

                    </div>
                </Fragment>

            );
        }
        if (type === 'email' && valueType === 'value') {
            const divClassName = this.props.divClassName ? this.props.divClassName : '';

            return (
                <Fragment>
                    <div className={'col mb-1 ' + divClassName + ' ' + colSize} >
                        {hasLabel && <label className={labelClass || ''}>{label}</label>}
                        {prefixIcon && <i class="tiny material-icons prefix customIcon valueType">
                            <img alt='' class="imageStyles" src={prefixIcon} />
                        </i>
                        }
                        <input
                            id={htmlFor}
                            max={max || null}
                            min={min || null}
                            maxLength={maxLength || null}
                            minLength={minLength || null}
                            name={name || null}
                            placeholder={placeholder || null}
                            required={required || null}
                            step={step || null}
                            autoComplete={autocomplete || null}
                            type={(isVisable ? 'text' : type) || 'text'}
                            className={"browser-default emailtextbox " + inputClass || "browser-default"}
                            onChange={onValueChange || null}
                            onKeyPress={onValueKeypress || null}
                            onKeyDown={type === 'number' ? this.onNumericKeypress : onValueKeyDown || null}
                            onBlur={onValueBlur || null}
                            onFocus={onValueFocus || null}
                            // disabled={disabled || null}
                            onInput={onValueInput || null}
                            defaultValue={defaultValue || null}
                            data-value={dataValType || null}
                            value={value}
                            readOnly={readOnly || null}
                            ref={refProps || null}
                        />
                    </div>
                </Fragment>

            );
        }
        if ((type === 'text' && dataType === 'numeric' && valueType !== 'value')) {

            const divClassName = this.props.divClassName ? this.props.divClassName : '';

            return (
                <Fragment>
                    <div className={'col mb-1 ' + divClassName + ' ' + colSize} >
                        {hasLabel && <label className={labelClass || ''}>{label}</label>}
                        {prefixIcon && <i className="tiny material-icons prefix customIcon">
                            <img alt='' className="imageStyles" src={prefixIcon} />
                        </i>
                        }
                        <input
                            id={htmlFor}
                            max={max || null}
                            min={min || null}
                            maxLength={maxLength || null}
                            minLength={minLength || null}
                            name={name || null}
                            placeholder={placeholder || null}
                            required={required || null}
                            step={step || null}
                            autoComplete={autocomplete || null}
                            type={(isVisable ? 'text' : type) || 'text'}
                            className={"browser-default numeric " + inputClass || "browser-default"}
                            onChange={onValueChange || null}
                            //onKeyPress={onValueKeypress || null}
                            //onKeyDown={onValueKeyDown || null}
                            onBlur={onValueBlur || null}
                            onFocus={onValueFocus || null}
                            // disabled={disabled || null}
                            onInput={this.numericFormat || null}
                            defaultValue={defaultValue || null}
                            value={value || null}
                            readOnly={readOnly || null}
                            ref={refProps || null}                                                  
                        />
                        {(type === 'password' || dataTypePassword === 'text') &&
                            <i className="tiny material-icons passwordIcon" onClick={showPasswordToggle}>
                                <img alt='' src={isVisable ? "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAALEwAACxMBAJqcGAAAAetJREFUSInt1M2LzlEUB/DPzDNlM+P9ZUzYeKnZaKQkhlIsyUYjfwELC4OxNMnGRhY2UrZiJopQlgqThZfUaKKJEY0SSooZZizueTx3fp7HjKnZyKlbv3vO9/s953fuvYf/NsO2YCbFV2MEPVhWC1Q3iUgzdqEdazA//O8xF62x78Dlv6muFZfwHeOTrAeon6pwA7oxWhAZxD4sxfFCbEvG70Cplvhs3A7SC/TG9wAWYiPOR7Ky+NWMvz18N9BUFG9CXwAuohH3Y78D26r81ah00LlGT8Tu5UnqcT0CZ1X6+TVWCbf83vszmfieTOtcxG8G15FwXDHxsH7gG3biXUH8k8obaMSrjFfCtcAdhs94gzmFtj2rUnV5Hc1w3egvcOdhGB/heVTaVgCdqCE+iFmBacGXSJLbeumM+kkHOBbExYUq3lZJ0JFhLmCo8PfNeCm1eGvZ2RXkJwEo2zoT+9+n8vrbpNauzfAteBrYzvyX6nAqAkPYFP4SHmcJNmec3ViU7dvxOnAnVRlDdTgkjYcx6body8R7i4SwFYEdk/p+sAbul23AIxP7PoKVEW+QXm2n9D7K8+qhdLhTspLKqBjH6Sx2oJD8DvaqMfD+NK6HsUS6y6vwIfxdUcAA7kqXYFq2XLqG+6cr8G/YT2VnofdMiQtDAAAAAElFTkSuQmCC" : "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAQAAABKfvVzAAAABGdBTUEAALGPC/xhBQAAACBjSFJNAAB6JgAAgIQAAPoAAACA6AAAdTAAAOpgAAA6mAAAF3CculE8AAAAAmJLR0QAAKqNIzIAAAAJcEhZcwAADdcAAA3XAUIom3gAAAAHdElNRQfiCAYSNTQeZEUpAAAA6klEQVQ4y+XSMWoCARCF4U9IIZhoZxGbXMBziLDkANEjbG0TUqQQw/Y29jmCJ7AI2FhsLqBYCFYpYhWcNMZsghghpTPd8Abmf284wyodmNU0NZHLvR1fbxiZi0LPjTQOi6v6NsKHMJHJTIStsNFX/S3vWgszLS23+2liZWAmrHWL8p6w0NkRlaRe5VJliaWajoXQ+5JnwlQdN67B/Y4gxUQbdVMhg6EwVgFPHvHi2VoIOTIPoGIsDC9+kLy7wqW742Z+n9Q2Qbo39eBJReiapURZKj8GXbR1YCX529ZicNvTgjv5Nf75fGdSn/ZoeJjRuTkYAAAAJXRFWHRkYXRlOmNyZWF0ZQAyMDE4LTA4LTA2VDE4OjUzOjUyKzAyOjAwsRD/MgAAACV0RVh0ZGF0ZTptb2RpZnkAMjAxOC0wOC0wNlQxODo1Mzo1MiswMjowMMBNR44AAAAZdEVYdFNvZnR3YXJlAHd3dy5pbmtzY2FwZS5vcmeb7jwaAAAAAElFTkSuQmCC"} />
                            </i>
                        }
                        {errorMsg && <span className="helper-text" data-error={errorMsg}></span>}
                    </div>
                </Fragment>

            );
        };
        if ((dataType === 'decimal' && valueType !== 'value')) {

            const divClassName = this.props.divClassName ? this.props.divClassName : '';

            return (
                <Fragment>
                    <div className={'col mb-1 ' + divClassName + ' ' + colSize} >
                        {hasLabel && <label className={labelClass || ''}>{label}</label>}
                        {prefixIcon && <i className="tiny material-icons prefix customIcon">
                            <img alt='' className="imageStyles" src={prefixIcon} />
                        </i>
                        }
                        <input
                            id={htmlFor}
                            max={max || null}
                            min={min || null}
                            maxLength={maxLength || null}
                            minLength={minLength || null}
                            name={name || null}
                            placeholder={placeholder || null}
                            required={required || null}
                            step={step || null}
                            autoComplete={autocomplete || null}
                            type={(isVisable ? 'text' : type) || 'text'}
                            className={"browser-default decimal " + inputClass || "browser-default"}
                            onChange={onValueChange || null}
                            //onKeyPress={onValueKeypress || null}
                            //onKeyDown={onValueKeyDown || null}
                            onBlur={onValueBlur || null}
                            onFocus={onValueFocus || null}
                            // disabled={disabled || null}
                            onInput={this.decimalWithLimtFormat || null}
                            defaultValue={defaultValue || null}
                            //    value={value || null}
                            readOnly={readOnly || null}
                            ref={refProps || null}
                        />
                        {errorMsg && <span className="helper-text" data-error={errorMsg}></span>}
                    </div>
                </Fragment>

            );
        }

        if ((dataType === 'decimal' && valueType === 'value')) {

            const divClassName = this.props.divClassName ? this.props.divClassName : '';

            return (
                <Fragment>
                    <div className={'col mb-1 ' + divClassName + ' ' + colSize} >
                        {hasLabel && <label className={labelClass || ''}>{label}</label>}
                        {prefixIcon && <i className="tiny material-icons prefix customIcon">
                            <img alt='' className="imageStyles" src={prefixIcon} />
                        </i>
                        }
                        <input
                            id={htmlFor}
                            max={max || null}
                            min={min || null}
                            maxLength={maxLength || null}
                            minLength={minLength || null}
                            name={name || null}
                            placeholder={placeholder || null}
                            required={required || null}
                            step={step || null}
                            autoComplete={autocomplete || null}
                            type={(isVisable ? 'text' : type) || 'text'}
                            className={"browser-default valueType " + inputClass || "browser-default"}
                            onChange={type === 'number' ? onValueChange : onValueChange || null}
                            //onKeyPress={onValueKeypress || null}
                            //onKeyDown={type === 'number' ? this.onNumericKeypress : onValueKeyDown || null}
                            onBlur={onValueBlur || null}
                            onFocus={onValueFocus || null}
                            // disabled={disabled || null}
                            onInput={isLimitType ?  this.decimalWithLimtFormat : this.decimalFormat || null}
                            //defaultValue={defaultValue || null }
                            prefixLimit={prefixLimit}
                            suffixLimit={suffixLimit}
                            data-value={dataType || null}
                            value={value}
                            readOnly={readOnly || null}
                            ref={refProps || null}
                        />
                        {errorMsg && <span className="helper-text" data-error={errorMsg}></span>}
                    </div>
                </Fragment>

            );
        };

        if ((dataType === 'numeric' && valueType === 'value')) {
            const divClassName = this.props.divClassName ? this.props.divClassName : '';
            return (
                <Fragment>
                    <div className={'col mb-1 ' + divClassName + ' ' + colSize} >
                        {hasLabel && <label className={labelClass || ''}>{label}</label>}
                        {prefixIcon && <i class="tiny material-icons prefix customIcon">
                            <img alt='' class="imageStyles" src={prefixIcon} />
                        </i>
                        }
                        <input
                            id={htmlFor}
                            max={max || null}
                            min={min || null}
                            maxLength={maxLength || null}
                            minLength={minLength || null}
                            name={name || null}
                            placeholder={placeholder || null}
                            required={required || null}
                            step={step || null}
                            autoComplete={autocomplete || null}
                            type={(isVisable ? 'text' : type) || 'text'}
                            className={"browser-default valueType numeric " + inputClass || "browser-default"}
                            onChange={type === 'number' ? onValueChange : onValueChange || null}
                            //onKeyPress={onValueKeypress || null}
                            //onKeyDown={type === 'number' ? this.onNumericKeypress : onValueKeyDown || null}
                            onBlur={onValueBlur || null}
                            onFocus={onValueFocus || null}
                            // disabled={disabled || null}
                            onInput={this.numericFormat || null}
                            //defaultValue={defaultValue || null }
                            data-value={dataType || null}
                            value={value}
                            readOnly={readOnly || null}
                            ref={refProps || null}
                        />
                        {errorMsg && <span className="helper-text" data-error={errorMsg}></span>}
                    </div>
                </Fragment>
            );
        }

        if (type === 'select') {
            return (
                <Fragment>
                    {/* <div className={`col mb-1 ${ divClassName } ${ colSize }`}> */}
                    <div className={'col mb-1 ' + divClassName + ' ' + colSize}>
                        {
                            hasLabel && <label className={'customLabel' + ' ' + labelClass}>{label}</label>
                        }
                        <select required={required || null} className={'browser-default customInputs' + ' ' + inputClass} id={id} name={name} onChange={onSelectChange} onBlur={onValueBlur} disabled={disabled} ref={refProps || null}> 
                            {/* {!defaultValue || defaultValue === undefined ? <option value="" default="true" selected >Select</option> : null} */}
                            {!selectstatus ? <option value="" default="true" selected>{optionSelecetLabel === '' ? 'Select' : optionSelecetLabel}</option> : null}
                            {optionsList && optionsList.map((dropdown, index) =>
                               <option
                               id={dropdown[optionName]}
                               key={index}
                               customId={dropdown[customId]} // to bind ID property which will be needed for reporting param.
                               className={optionClassArray && optionClassArray.includes(dropdown[optionProperty]) ? optionClassName : ""}
                               value={dropdown[optionValue]}
                               title={isTooltip && dropdown[optionName]}
                               //selected={dropdown[optionValue] == defaultValue ? true : false}
                               selected={customDefaultValueCheck ?
                                   dropdown[optionValue] + defaultValueCheckComparator + dropdown[optionName] == defaultValue ? true:false:
                                   dropdown[optionValue]===0?dropdown[optionValue] === defaultValue ? true : false:dropdown[optionValue] == defaultValue ? true : false
                               }
                           >{dropdown[optionName]}
                           </option>
                            )}
                        </select>
                    </div>

                </Fragment>
            );
        }

        if (type === 'selectOptionGroup') {
            return (
                <Fragment>
                    <div className={'col mb-1 ' + divClassName + ' ' + colSize} >
                        {
                            hasLabel && <label className="customLabel">{label}</label>
                        }
                        <select value={defaultValue} className="browser-default customInputs"
                            onChange={onSelectGroupChange} disabled={disabled} ref={ref || null}
                        >
                            {!defaultValue ? <option disabled={disabled}>Select</option> : null}
                            {
                                optionGroups.map((optGrop, index) => {
                                    return (
                                        <optgroup label={optGrop[optionGroupLabelKey]} key={index}>
                                            {optGrop[subOption].map((option, i) =>
                                                <option key={i} value={option[optionKeyValue]}>{option[optionKeyValue]}</option>
                                            )}
                                        </optgroup>
                                    );
                                }

                                )
                            }
                        </select>
                    </div>
                </Fragment>
            );
        }
        if (type === 'multiSelect') {
            const colourStyles = {
                control: styles => ({ ...styles, backgroundColor: "white" }),          
                option: (styles, { data }) => {
                    return { ...styles, color: data.color, ':active': { ...styles[':active'] } };
                },      
                multiValue: (styles, { data }) => {                                    
                  return {
                    ...styles,
                    color: data.color!==undefined ? data.color : '#000'
                  };
                },
                multiValueLabel: (styles, { data } ) => {                   
                return {
                  ...styles,
                  color: data.color!==undefined ? data.color : '#000'
                    };
                }                
              };
            return (
                <div className={'col mb-1 ' + divClassName + ' ' + colSize} >
                    {
                        //hasLabel && <label className="customLabel">{label}</label>
                        hasLabel && <label className={labelClass || ''}>{label}</label>
                    }
                    {valueType === 'defaultValue' ? <Select
                        isDisabled={disabled}
                        closeMenuOnSelect={false}
                        isMulti={true}
                        options={optionsList}
                        defaultValue={this.setMultiSelectedValue(optionsList,
                            defaultValue)}
                        onChange={this.onChangeMultiSelectdValue}
                        ref={refProps}  //D219
                    /> : <Select
                            isDisabled={disabled}
                            closeMenuOnSelect={false}
                            isMulti={true}
                            options={optionsList}
                            value={this.setMultiSelectedValue(optionsList,
                                defaultValue)}
                            onChange={this.onChangeMultiSelectdValue}
                            styles={colourStyles}
                            
                        />}

                </div>
            );
        }

        if (type === 'reactMultiSelect') {
            const optionsArray = [];
            optionsList && optionsList.map((option) => {
                return (
                    optionsArray.push({ 'label': option[optionName], 'value': option[optionValue] })
                );
            });
            const defaultValueArray = [];
            defaultValue && defaultValue.map((defaultOption) => {
                return (
                    defaultValueArray.push({ 'label': defaultOption[optionName], 'value': defaultOption[optionValue] })
                );
            });
            return (
                <div className={'col mb-1 ' + divClassName + ' ' + colSize} >
                    {
                        //hasLabel && <label className="customLabel">{label}</label>
                        hasLabel && <label className={labelClass || ''}>{label}</label>
                    }
                    <MultiSelect
                        options={optionsArray}
                        onValuesChange={onValueChange}
                        isDisabled={disabled}
                        values={defaultValueArray || ''}
                        name={name}
                    />
                </div>
            );
        }

        if (type === 'checkbox') {
            return (
                <Fragment>

                    <div className={'col mb-1' + divClassName + ' ' + colSize}>
                        {
                            hasLabel && <label className={"customLabel" + labelClass}>{label}</label>
                        }
                        {checkBoxArray.map(checkbox => {
                            return (
                                <label key={checkbox.value}>
                                    <input
                                        type="checkbox"
                                        checked={checkbox.value || null}
                                        id={id || null }
                                        // disabled={checkbox.disabled}
                                        disabled={disabled || null}
                                        value={checkbox.value}
                                        name={name || null}
                                        className="filled-in"
                                        onChange={onCheckboxChange}
                                        ref={refProps || null}
                                    />
                                    <span className={ labelClass?labelClass:'label-bold lh-inherit' } >{checkbox.label}</span>
                                </label>
                            );
                        })}
                    </div>

                </Fragment>
            );
        }
        if (type === 'switch') {
            return (
                <Fragment>
                     <div className={'switch col' + ' ' + colSize} > 
                        {isSwitchLabel && <label className={"customLabel" + ' ' + labelClass}>{switchLabel}
                            <input type="checkbox" className={switchInputClass} key={switchKey} class="switchfocus"  disabled={disabled} id={ htmlFor || id } name={switchName} onChange={onChangeToggle} defaultChecked={checkedStatus}  ref={refProps || null} />
                            <span className={"lever" + ' ' + switchSpanClass}></span>
                        </label>}
                        {!isSwitchLabel &&
                            <label>
                                <input type="checkbox" id={ htmlFor || id } name={switchName} class="switchfocus" disabled={disabled} onChange={onChangeToggle} defaultChecked={checkedStatus}  ref={refProps || null} />
                                <span className="lever"></span></label>
                        }
                    </div>
                </Fragment>
            );
        }

        if (type === 'radio') {
            return (
                <Fragment>
                    <div className={'col mb-1' + divClassName + ' ' + colSize}>

                        {
                            hasLabel && <label className={"customLabel" + labelClass}>{label}</label>
                        }
                        {optionsArray.map(radio => {
                            return (

                                <label key={radio[value]}>
                                    <input
                                        type="radio"
                                        checked={defaultValue === radio[value]}
                                        disabled={radio[disabled]}
                                        value={radio[value]}
                                        name={radio[value]}
                                        className="with-gap"
                                        onChange={onRadioChange}
                                        ref={ref || null}
                                    />
                                    <span>{radio[name]}</span>
                                </label>
                            );
                        })}
                    </div>
                </Fragment>
            );
        }

        if (type === 'date') {
            return (
                <Fragment>
                    {isNonEditDateField && dateUtil.formatDate(this.props.data[this.props.dataToRender], '-')}
                    <div className={'col mb-1' + divClassName + ' ' + colSize}>

                        {
                            hasLabel && <label className={"customLabel" + ' ' + labelClass}>{label}</label>
                        }
                        {!isNonEditDateField &&
                            <div className="datePickerHolder">
                                <DatePicker
                                    popperContainer={ CalendarContainer }
                                    showYearDropdown
                                    scrollableYearDropdown
                                    showMonthDropdown
                                    yearDropdownItemNumber={5}
                                    autoComplete="off"
                                    name={name}
                                    selected={selectedDate}
                                    onBlur={onDatePickBlur}
                                    dateFormat={dateFormat}
                                    shouldCloseOnSelect={shouldCloseOnSelect}
                                    className={"browser-default" + ' ' + inputClass}
                                    onChange={onDateChange}
                                    disabled={disabled || null}
                                    placeholderText={localConstant.commonConstants.UI_DATE_FORMAT}
                                    disabledKeyboardNavigation
                                    onChangeRaw={(e) => { this.handleDateChangeRaw(e); }}
                                    // customInput={<MaskedInput
                                    //     keepCharPositions={true}
                                    //     mask={ [ /\d/, /\d/, '-', /\d/, /\d/, '-', /\d/, /\d/, /\d/, /\d/ ] }
                                    // />}
                                />
                            </div>
                        }
                    </div>
                </Fragment>
            );
        }

        if (type === 'inlineDatepicker') {
            return (
                <Fragment>                 
                    <div className={'col mb-1' + divClassName + ' ' + colSize + ' pl-0 pr-0'}>
                        {!isNonEditDateField &&
                            <div className="datePickerHolder">                                
                                <DatePicker
                                    id={htmlFor}
                                    popperContainer={ CalendarContainer }
                                    popperPlacement="top-start"                                   
                                    showYearDropdown
                                    scrollableYearDropdown
                                    showMonthDropdown
                                    yearDropdownItemNumber={3}
                                    autoComplete="off"
                                    name={name}
                                    selected={selectedDate}
                                    onBlur={onDatePickBlur}
                                    dateFormat={dateFormat}                                    
                                    className={"browser-default" + ' ' + inputClass}
                                    onChange={onDateChange}
                                    onSelect={onDateSelect}
                                    disabled={disabled || null}
                                    placeholderText={localConstant.commonConstants.UI_DATE_FORMAT}
                                    disabledKeyboardNavigation
                                    onChangeRaw={(e) => { this.handleDateChangeRaw(e); }}                                   
                                />
                            </div>
                        }
                    </div>
                </Fragment>
            );
        }

        if (type === 'datetime') {
            return (
                <Fragment>
                    {isNonEditDateField && dateUtil.formatDate(this.props.data[this.props.dataToRender], '-')}
                    <div className={'col mb-1' + divClassName + ' ' + colSize}>

                        {
                            hasLabel && <label className={"customLabel" + ' ' + labelClass}>{label}</label>
                        }
                        {!isNonEditDateField &&
                            <div className="datePickerHolder">
                                <DatePicker
                                    showYearDropdown
                                    scrollableYearDropdown
                                    showMonthDropdown
                                    yearDropdownItemNumber={3}
                                    autoComplete="off"
                                    name={name}
                                    selected={selectedDate}
                                    onBlur={onDatePickBlur}
                                    dateFormat={dateFormat}
                                    shouldCloseOnSelect={shouldCloseOnSelect}
                                    className="browser-default"
                                    onChange={onDateChange}
                                    disabled={disabled || null}
                                    // readOnly={readOnly||null}
                                    placeholderText={placeholderText}
                                    disabledKeyboardNavigation
                                    onChangeRaw={(e) => { this.handleDateChangeRaw(e); }}
                                    // customInput={<MaskedInput
                                    //     keepCharPositions={true}
                                    //     mask={ [ /\d/, /\d/, '-', /\d/, /\d/, '-', /\d/, /\d/, /\d/, /\d/ ] }
                                    // />}
                                    showTimeSelect
                                    showTimeSelectOnly
                                    timeFormat={timeFormat}
                                    timeIntervals={timeIntervals}
                                    timeCaption={timeCaption}  
                                    minTime={minTime}
                                    maxTime={maxTime}
                                  
                                />
                            </div>
                        }
                    </div>
                </Fragment>
            );
        }

        if (type === 'range') {
            return (
                <Fragment>
                    <div className={'col mb-1 ' + divClassName + ' ' + colSize} >
                        {hasLabel && <Fragment><label className={labelClass || ''}>{label}</label><span className="badge">{percentageDisplayValue}%</span></Fragment>}
                        <input
                            id={htmlFor}
                            max={max || null}
                            min={min || null}
                            onInput={onValueInput || null}
                            name={name || null}
                            step={step || null}
                            type={'range'}
                            onChange={onValueChange || null}
                            disabled={disabled || null}
                            value={value || undefined}
                        />
                    </div>
                </Fragment>

            );
        }

        if (type === 'selectOptionGroup') {
            return (
                <Fragment>
                    <div className={'col mb-1 ' + divClassName + ' ' + colSize} >
                        {
                            hasLabel && <label className={"customLabel" + ' ' + labelClass} >{label}</label>
                        }
                        <select id={htmlFor} name={name} ref={onRef} className="browser-default customInputs"
                            onChange={onSelectGroupChange} disabled={disabled}
                        >
                            {/* {!defaultValue ? <option value="" disabled={disabled}>Select</option> : null} */}
                            <option value="" disabled={disabled}>Select</option>

                            {
                                optionGroups.map((optGrop, index) => {
                                    return (
                                        <optgroup label={optGrop[optionGroupLabelKey]} key={index}>
                                            {optGrop[subOption].map((option, i) =>
                                                <option key={i}
                                                    value={option[optionKeyValue]}
                                                    selected={option[optionKeyName] === defaultValue ? true : false}
                                                >{option[optionKeyName]}</option>
                                            )}
                                        </optgroup>
                                    );
                                }

                                )
                            }
                        </select>
                    </div>
                </Fragment>
            );
        }

    }

}
export default CustomInput;

CustomInput.propTypes = {
    inputModal: PropTypes.string.isRequired,
    type: PropTypes.string.isRequired,
    inputPrefixeIcon: PropTypes.string,
    labelName: PropTypes.string,
    inputName: PropTypes.string,
    disabled: PropTypes.bool,
    colSize: PropTypes.string,
    className: PropTypes.string,
    errorMsg: PropTypes.string,
    onValueChange: PropTypes.func,
    optionsList: PropTypes.array,
    onSelectChange: PropTypes.func,
    optionGroups: PropTypes.array,
    onSelectGroupChange: PropTypes.func,
    onChangeToggle: PropTypes.func,
    radioName: PropTypes.string,
    radioGroup: PropTypes.array,
    onRadioChange: PropTypes.func,
    isNonEditDateField: PropTypes.bool,
    onCheckBoxChange: PropTypes.func,
    optionsArray: PropTypes.array,
    onDateChange: PropTypes.func,
    dataType: PropTypes.string,
    toFixedlimit: PropTypes.number,
    valueType: PropTypes.string,
    isLimitType: PropTypes.bool,
    prefixLimit: PropTypes.number,
    suffixLimit: PropTypes.number,
    onValueBlur:PropTypes.func  //D-675 Changes

};

CustomInput.defaultProps = {
    colSize: 's12',
    inputModal: 'default',
    divClassName: '',
    labelName: '',
    type: 'text',
    dataValType: 'text',
    dataType: 'text',
    disabled: false,
    optionsList: [],
    optionGroups: [],
    radioGroup: [],
    optionsArray: [],
    checkBoxArray: [],
    isNonEditDateField: false,
    labelClass: '',
    inputClass: '',
    optionSelecetLabel: '',
    dataTypePassword: '',
    toFixedlimit: 2,
    valueType: 'default',
    isLimitType: false,
    prefixLimit: 10,
    suffixLimit: 2,
    switchSpanClass:''

}; 