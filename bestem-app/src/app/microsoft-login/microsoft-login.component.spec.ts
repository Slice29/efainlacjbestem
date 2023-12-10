import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MicrosoftLoginComponent } from './microsoft-login.component';

describe('MicrosoftLoginComponent', () => {
  let component: MicrosoftLoginComponent;
  let fixture: ComponentFixture<MicrosoftLoginComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MicrosoftLoginComponent]
    });
    fixture = TestBed.createComponent(MicrosoftLoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
