import pygame, shared

class Shield():
	def __init__(self, x, y, line_colour, fill_colour) -> None:
		self.BAR_LENGTH = 100
		self.BAR_HEIGHT = 10
		self.line_colour = line_colour
		self.fill_colour = fill_colour
		self.pct = 0
		self.outline_rect = pygame.Rect(x, y, self.BAR_LENGTH, self.BAR_HEIGHT)
		self.fill_rect = pygame.Rect(x, y, self.BAR_LENGTH, self.BAR_HEIGHT)	
		
	def update(self, value) -> None:
		if value < 0:
			value = 0	
		self.pct = value
		self.fill_rect.width = (self.pct / 100) * self.BAR_LENGTH
	
	def draw(self) -> None:
		pygame.draw.rect(shared.screen, self.fill_colour, self.fill_rect)
		pygame.draw.rect(shared.screen, self.line_colour, self.outline_rect, 2) #2 pixel thick line