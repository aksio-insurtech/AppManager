/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environment');

export interface IAddEnvironment {
    applicationId?: string;
}

export class AddEnvironmentValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        applicationId: new Validator(),
    };
}

export class AddEnvironment extends Command<IAddEnvironment> implements IAddEnvironment {
    readonly route: string = '/api/applications/{{applicationId}}/environment';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new AddEnvironmentValidator();

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

    static use(initialValues?: IAddEnvironment): [AddEnvironment, SetCommandValues<IAddEnvironment>] {
        return useCommand<AddEnvironment, IAddEnvironment>(AddEnvironment, initialValues);
    }
}
