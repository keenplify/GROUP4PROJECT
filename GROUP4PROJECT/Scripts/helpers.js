function tryParseLocalStorageObject(key) {
    try {
        const value = localStorage.getItem(key);
        const object = JSON.parse(value);

        return object;
    } catch (e) {
        console.warn("Error while parsing LS object " + key, e);
    }
}

function trySaveLocalStorageObject(key, value) {
    try {
        localStorage.setItem(key, JSON.stringify(value));
    } catch (e) {
        console.warn("Error while setting LS object " + key, e);
    }
}
