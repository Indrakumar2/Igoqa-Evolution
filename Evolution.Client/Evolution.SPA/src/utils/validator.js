export const required = (value) => (!value || value === '' || (value.toString().toLowerCase()) === 'select');
export const requiredNumeric = (value) => (isNaN(parseFloat(value)));
export const EMailRegex = (email) => {
    const emailValue = email.match(/^([\w.%+-]+)@([\w-]+\.)+([\w]{1,})$/i);
    if (emailValue == null) {
        return true;
    }
    else
        return false;
};

/** string has only spaces */
export const stringWithOnlySpaces = (value) => {
    return value && !value.replace(/\s/g, '').length;
};