import pygame, shared

class Shield():
	'''
	draw 3 rectangles:
	1. solid background colour -> shared.DARK_BLUE
	2. solid green (width set by shield value)
	3. outline in white
	'''
	def __init__(self, x:int, y:int, line_colour:tuple, fill_colour:tuple) -> None:
		self.BAR_LENGTH:int      = 100
		self.BAR_HEIGHT:int      = 10
		self.line_colour:tuple   = line_colour
		self.fill_colour:tuple   = fill_colour
		self.back_colour:tuple	 = shared.DARK_BLUE
		self.pct:int             = 0
		# create 3 rectangles
		self.bg_rect:object      = pygame.Rect(x, y, self.BAR_LENGTH, self.BAR_HEIGHT)
		self.outline_rect:object = self.bg_rect.copy()	# copy the rectangle else it refers to same rectangle!
		self.fill_rect:object    = self.bg_rect.copy()
		
	def update(self, value) -> None:
		''' change the bar length as a proportion of player.shield '''
		if value < 0:
			value = 0	
		self.pct = value
		self.fill_rect.width = (self.pct / 100) * self.BAR_LENGTH
	
	def draw(self) -> None:
		pygame.draw.rect(shared.screen, self.back_colour, self.bg_rect)			# dark background
		pygame.draw.rect(shared.screen, self.fill_colour, self.fill_rect)		# green shield value
		pygame.draw.rect(shared.screen, self.line_colour, self.outline_rect, 2) # 2 pixel thick outline