/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues, ClearCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/config');

export interface ISetConfigForApplication {
    applicationId?: string;
    name?: string;
    content?: string;
}

export class SetConfigForApplicationValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        applicationId: new Validator(),
        name: new Validator(),
        content: new Validator(),
    };
}

export class SetConfigForApplication extends Command<ISetConfigForApplication> implements ISetConfigForApplication {
    readonly route: string = '/api/applications/{{applicationId}}/config';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new SetConfigForApplicationValidator();

    private _applicationId!: string;
    private _name!: string;
    private _content!: string;

    get requestArguments(): string[] {
        return [
            'applicationId',
        ];
    }

    get properties(): string[] {
        return [
            'applicationId',
            'name',
            'content',
        ];
    }

    get applicationId(): string {
        return this._applicationId;
    }

    set applicationId(value: string) {
        this._applicationId = value;
        this.propertyChanged('applicationId');
    }
    get name(): string {
        return this._name;
    }

    set name(value: string) {
        this._name = value;
        this.propertyChanged('name');
    }
    get content(): string {
        return this._content;
    }

    set content(value: string) {
        this._content = value;
        this.propertyChanged('content');
    }

    static use(initialValues?: ISetConfigForApplication): [SetConfigForApplication, SetCommandValues<ISetConfigForApplication>, ClearCommandValues] {
        return useCommand<SetConfigForApplication, ISetConfigForApplication>(SetConfigForApplication, initialValues);
    }
}
