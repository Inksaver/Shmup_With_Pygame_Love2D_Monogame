''' Circle class to provdide circle collider'''
class Circle():
	def __init__(self, x:int, y:int, radius:int) -> None:
		self._x = x				# x position
		self._y = y				# y position
		self._radius = radius	# circle radius
		
	@property
	def x(self):
		return self._x
	
	@property
	def y(self):
		return self._y
	
	@property
	def radius(self):
		return self._radius
	
	@x.setter
	def x(self, value):
		self._x = value
		
	@y.setter
	def y(self, value):
		self._y = value	
		
	@radius.setter
	def radius(self, value):
		self._radius = value
	
	@property
	def center(self):
		return [self._x, self._y]
	
	@center.setter
	def center(self, value:list):
		self._x = value[0]
		self._y = value[1]	
	