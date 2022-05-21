/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/remove');

export interface IRemove {
    applicationId?: string;
}

export class RemoveValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        applicationId: new Validator(),
    };
}

export class Remove extends Command<IRemove> implements IRemove {
    readonly route: string = '/api/applications/{{applicationId}}/remove';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new RemoveValidator();

    private _applicationId!: string;

    get requestArguments(): string[] {
        return [
            'applicationId',
        ];
    }

    get properties(): string[] {
        return [
            'applicationId',
        ];
    }

    get applicationId(): string {
        return this._applicationId;
    }

    set applicationId(value: string) {
        this._applicationId = value;
        this.propertyChanged('applicationId');
    }

    static use(initialValues?: IRemove): [Remove, SetCommandValues<IRemove>] {
        return useCommand<Remove, IRemove>(Remove, initialValues);
    }
}
