/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/stack');

export interface ISetStack {
    applicationId?: string;
    json?: string;
}

export class SetStackValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        applicationId: new Validator(),
        json: new Validator(),
    };
}

export class SetStack extends Command<ISetStack> implements ISetStack {
    readonly route: string = '/api/applications/{{applicationId}}/stack';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new SetStackValidator();

    private _applicationId!: string;
    private _json!: string;

    get requestArguments(): string[] {
        return [
            'applicationId',
        ];
    }

    get properties(): string[] {
        return [
            'applicationId',
            'json',
        ];
    }

    get applicationId(): string {
        return this._applicationId;
    }

    set applicationId(value: string) {
        this._applicationId = value;
        this.propertyChanged('applicationId');
    }
    get json(): string {
        return this._json;
    }

    set json(value: string) {
        this._json = value;
        this.propertyChanged('json');
    }

    static use(initialValues?: ISetStack): [SetStack, SetCommandValues<ISetStack>] {
        return useCommand<SetStack, ISetStack>(SetStack, initialValues);
    }
}
