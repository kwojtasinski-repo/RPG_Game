export default function mapExceptionToMessage(exception) {
    const somethingBadHappen = 'Something went wrong';
    if (exception === null || exception === undefined) {
        return somethingBadHappen;
    }

    if (exception.response === null || exception.response === undefined) {
        return somethingBadHappen;
    }

    if (exception.response.data === null || exception.response.data === undefined) {
        return getMessageFromStatusCode(exception.response.status);
    }

    if (!exception.response.data.message) {
        return getMessageFromStatusCode(exception.response.status);
    }

    return exception.response.data.message;
}

function getMessageFromStatusCode(status) {
    const genericMessage = 'An error occured';
    if (status === 404) {
        return 'Not found';
    } else {
        return genericMessage;
    }
}
